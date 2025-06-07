$(document).ready(function () {
    // Отключаем предыдущие обработчики событий
    $(document).off('click', '.add-friend-btn');
    $(document).off('click', '.accept-friend-btn');
    $(document).off('click', '.reject-friend-btn');
    $(document).off('click', '.remove-friend-btn');

    // Отключаем предыдущие обработчики SignalR событий
    $(document).off('friendRequest');
    $(document).off('friendRequestAccepted');
    $(document).off('friendRequestRejected');
    $(document).off('friendRemoved');

    function updateFriendStatus(userId, status, senderData = null) {
        console.log('Updating friend status:', { userId, status, senderData });
        const isUserPage = window.location.pathname.includes('/Users/UserPage/');
        const actions = {
            'pending': isUserPage ? `
                <a href="#" class="mt-2 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-warning font-xsssss fw-700 ls-lg text-white">
                    <i class="feather-clock font-xss"></i> PENDING
                </a>` : `
                <a href="/Users/UserPage/${senderData?.id || userId}" class="mt-0 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white">
                    <i class="feather-user font-xss"></i> PROFILE
                </a>
                <a href="#" class="mt-2 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-warning font-xsssss fw-700 ls-lg text-white">
                    <i class="feather-clock font-xss"></i> PENDING
                </a>`,
            'friend': isUserPage ? `
                <div class="d-flex align-items-center">
                    <a href="#" class="mt-3 p-0 btn p-2 lh-24 w100 ml-1 ls-3 d-inline-block rounded-xl bg-danger font-xsssss fw-700 ls-lg text-white btn-profile remove-friend-btn" data-user-id="${userId}">
                        <i class="feather-user-minus mr-1"></i> REMOVE FRIEND
                    </a>
                    <a href="#" class="btn-round-md ml-2 mt-2 d-inline-block float-right bg-greylight" data-toggle="tooltip" title="Send message">
                        <i class="feather-message-square font-sm text-grey-900"></i>
                    </a>
                </div>` : `
                <a href="/Users/UserPage/${senderData?.id || userId}" class="mt-0 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white">
                    <i class="feather-user font-xss"></i> PROFILE
                </a>
                <button class="mt-2 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-danger font-xsssss fw-700 ls-lg text-white remove-friend-btn" data-user-id="${userId}">
                    <i class="feather-user-minus font-xss"></i> REMOVE FRIEND
                </button>`,
            'none': isUserPage ? `
                <a href="#" class="mt-2 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white add-friend-btn" data-user-id="${userId}">
                    <i class="feather-user-plus font-xss"></i> ADD FRIEND
                </a>` : `
                <a href="/Users/UserPage/${senderData?.id || userId}" class="mt-0 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white">
                    <i class="feather-user font-xss"></i> PROFILE
                </a>
                <button class="mt-2 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white add-friend-btn" data-user-id="${userId}">
                    <i class="feather-user-plus font-xss"></i> ADD FRIEND
                </button>`,
            'request': isUserPage ? `
                <div class="d-flex">
                    <a href="#" class="mt-3 p-0 btn p-2 lh-24 w100 ml-1 ls-3 d-inline-block rounded-xl bg-success font-xsssss fw-700 ls-lg text-white btn-profile accept-friend-btn" data-user-id="${userId}">
                        <i class="feather-check mr-1"></i> ACCEPT
                    </a>
                    <a href="#" class="mt-3 p-0 btn p-2 lh-24 w100 ml-1 ls-3 d-inline-block rounded-xl bg-danger font-xsssss fw-700 ls-lg text-white btn-profile reject-friend-btn" data-user-id="${userId}">
                        <i class="feather-x mr-1"></i> REJECT
                    </a>
                </div>` : `
                <a href="/Users/UserPage/${senderData?.id || userId}" class="mt-0 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white">
                    <i class="feather-user font-xss"></i> PROFILE
                </a>
                <div class="d-flex justify-content-center">
                    <button class="mt-2 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white accept-friend-btn" data-user-id="${userId}">
                        <i class="feather-user-plus font-xss"></i> ACCEPT
                    </button>
                    <button class="mt-2 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-danger font-xsssss fw-700 ls-lg text-white reject-friend-btn" data-user-id="${userId}">
                        <i class="feather-user-minus font-xss"></i> REJECT
                    </button>
                </div>`
        };

        // Find user cards and elements
        const elements = $(`.user-card[data-user-id="${userId}"], .user-item[data-user-id="${userId}"], .friend-actions[data-user-id="${userId}"]`);
        console.log('Found elements:', elements.length);

        // If we can't find the user's card and this is a new friend request
        if (elements.length === 0 && status === 'request' && senderData) {
            console.log('Creating new friend request card');
            // Get the requests container
            const requestsSection = $('.pending-requests-container');
            if (requestsSection.length) {
                // Create the new user card
                const userCard = $(`
                    <div class="col-xl-4 col-lg-6 col-md-6 user-item" data-user-id="${senderData.senderId}">
                        <div class="card mb-4 d-block w-100 shadow-xss rounded-lg p-4 border-0 text-center user-card">
                            <a href="/Users/UserPage/${senderData.id}" class="ml-auto mr-auto rounded-lg overflow-hidden d-inline-block user-image">
                                <img src="/images/user-12.png" alt="${senderData.senderName}" class="p-0 w100 shadow-xss">
                            </a>
                            <h4 class="fw-700 font-xs mt-3 mb-1">${senderData.senderName}</h4>
                            <p class="fw-600 font-xssss text-grey-500 mt-0 mb-2">${senderData.senderEmail}</p>
                            <div class="clearfix"></div>
                            <div class="friend-actions" data-user-id="${senderData.senderId}">
                                <a href="/Users/UserPage/${senderData.id}" class="btn pt-2 pb-2 ps-3 pe-3 lh-24 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white">
                                    <i class="feather-user font-xss"></i> PROFILE
                                </a>
                                <div class="d-flex justify-content-center mt-2">
                                    <button class="btn pt-2 pb-2 ps-3 pe-3 lh-24 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white accept-friend-btn" data-user-id="${senderData.senderId}">
                                        <i class="feather-user-plus font-xss"></i> ACCEPT
                                    </button>
                                    <button class="btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-danger font-xsssss fw-700 ls-lg text-white reject-friend-btn" data-user-id="${senderData.senderId}">
                                        <i class="feather-user-minus font-xss"></i> REJECT
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                `);
                
                requestsSection.append(userCard);
                if (!$('.pending-requests-section').is(':visible')) {
                    $('.pending-requests-section').show();
                }
                return;
            }
        }

        // Update existing elements
        elements.each(function() {
            const element = $(this);
            const actionHtml = actions[status];
            console.log('Updating element with status:', status);

            if (actionHtml) {
                if (element.hasClass('friend-actions')) {
                    element.html(actionHtml);
                    // Reinitialize tooltips after updating content
                    if (status === 'friend' && isUserPage) {
                        $('[data-toggle="tooltip"]').tooltip();
                    }
                } else {
                    const actionsContainer = element.find('.friend-actions');
                    actionsContainer.html(actionHtml);
                    // Reinitialize tooltips after updating content
                    if (status === 'friend' && isUserPage) {
                        $('[data-toggle="tooltip"]').tooltip();
                    }
                }
            }

            // Update friend status icon
            const userName = element.find('.fw-700.font-xs');
            const checkIcon = userName.find('.feather-user-check');
            
            if (status === 'friend') {
                if (!checkIcon.length) {
                    userName.append('<i class="feather-user-check text-success ml-1" data-toggle="tooltip" title="Friend"></i>');
                    $('[data-toggle="tooltip"]').tooltip();
                }
            } else {
                checkIcon.remove();
                // If we're on MyProfile page and this was the last friend, show the empty message
                if (status === 'none' && window.location.pathname.includes('/Users/MyProfile')) {
                    element.closest('.user-item').fadeOut(300, function() {
                        $(this).remove();
                        if ($('.user-item').length === 0) {
                            const emptyMessage = `
                                <div class="col-12">
                                    <p class="text-center text-grey-500">You don't have any friends yet.</p>
                                </div>`;
                            $('.row:has(.user-item)').html(emptyMessage);
                        }
                    });
                }
            }
        });

        // Update any counters or badges
        updateFriendCounters();
    }

    function updateFriendCounters() {
        // Update friend count in the sidebar if it exists
        const friendCount = $('.friend-count');
        if (friendCount.length) {
            const currentCount = parseInt(friendCount.text()) || 0;
            friendCount.text(currentCount + 1);
        }
    }

    function handleFriendAction(userId, action, button) {
        console.log('Handling friend action:', { userId, action });
        const token = $('input[name="__RequestVerificationToken"]').val();
        console.log('Token:', token);
        button.prop('disabled', true);
        
        $.ajax({
            url: '/Users/HandleFriendRequest',
            type: 'POST',
            data: {
                targetUserId: userId,
                action: action,
                __RequestVerificationToken: token
            },
            success: function(response) {
                console.log('Friend action response:', response);
                button.prop('disabled', false);
                if (response.success) {
                    let newStatus = '';
                    switch (action) {
                        case 'accept':
                            newStatus = 'friend';
                            updateFriendStatus(userId, 'friend');
                            // Удаляем запись о запросе при принятии в друзья
                            const acceptedRequests = JSON.parse(localStorage.getItem('sentFriendRequests') || '{}');
                            delete acceptedRequests[userId];
                            localStorage.setItem('sentFriendRequests', JSON.stringify(acceptedRequests));
                            break;
                        case 'reject':
                        case 'remove':
                            newStatus = 'none';
                            updateFriendStatus(userId, 'none');
                            // Удаляем запись о запросе при отклонении или удалении
                            const rejectedRequests = JSON.parse(localStorage.getItem('sentFriendRequests') || '{}');
                            delete rejectedRequests[userId];
                            localStorage.setItem('sentFriendRequests', JSON.stringify(rejectedRequests));
                            break;
                    }
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function(xhr, status, error) {
                console.error('Friend action error:', { xhr, status, error });
                button.prop('disabled', false);
                toastr.error('There was an error processing your request');
            }
        });
    }

    // При загрузке страницы проверяем отправленные запросы
    function checkSentRequests() {
        console.log('Checking sent requests');
        const sentRequests = JSON.parse(localStorage.getItem('sentFriendRequests') || '{}');
        Object.keys(sentRequests).forEach(userId => {
            // Проверяем, не являемся ли мы уже друзьями
            const element = $(`.friend-actions[data-user-id="${userId}"]`);
            if (element.length) {
                // Если есть кнопка remove-friend-btn, значит мы уже друзья
                if (element.find('.remove-friend-btn').length) {
                    // Удаляем запись из localStorage
                    delete sentRequests[userId];
                    localStorage.setItem('sentFriendRequests', JSON.stringify(sentRequests));
                } else {
                    updateFriendStatus(userId, 'pending');
                }
            }
        });
    }

    // Вызываем проверку при загрузке страницы
    checkSentRequests();

    // Click handler for add friend button
    $(document).on('click', '.add-friend-btn', function (e) {
        e.preventDefault();
        const button = $(this);
        const userId = button.data('user-id');
        
        if (!userId) {
            console.error('User ID not found');
            toastr.error('Error: User ID not found');
            return;
        }

        // Disable the button to prevent double clicks
        button.prop('disabled', true);

        // Get the anti-forgery token
        const token = $('input[name="__RequestVerificationToken"]').val();

        // Send the friend request
        $.ajax({
            url: '/Users/HandleFriendRequest',
            type: 'POST',
            data: {
                targetUserId: userId,
                action: 'add',
                __RequestVerificationToken: token
            },
            success: function (response) {
                if (response.success) {
                    // Update the button to show pending state
                    updateFriendStatus(userId, 'pending');
                    toastr.success('Friend request sent successfully');
                    
                    // Save to localStorage
                    const sentRequests = JSON.parse(localStorage.getItem('sentFriendRequests') || '{}');
                    sentRequests[userId] = true;
                    localStorage.setItem('sentFriendRequests', JSON.stringify(sentRequests));
                } else {
                    toastr.error(response.message || 'Failed to send friend request');
                    // Re-enable the button on error
                    button.prop('disabled', false);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error sending friend request:', error);
                toastr.error('Error sending friend request');
                // Re-enable the button on error
                button.prop('disabled', false);
            }
        });
    });

    // Click handlers for friend-related buttons
    $(document).on('click', '.accept-friend-btn', function(e) {
        e.preventDefault();
        console.log('Accept friend button clicked');
        const userId = $(this).data('user-id');
        handleFriendAction(userId, 'accept', $(this));
    });

    $(document).on('click', '.reject-friend-btn', function(e) {
        e.preventDefault();
        console.log('Reject friend button clicked');
        const userId = $(this).data('user-id');
        handleFriendAction(userId, 'reject', $(this));
    });

    $(document).on('click', '.remove-friend-btn', function(e) {
        e.preventDefault();
        console.log('Remove friend button clicked');
        const userId = $(this).data('user-id');
        handleFriendAction(userId, 'remove', $(this));
    });

    // SignalR event handlers
    $(document).on("friendRequest", function (e, data) {
        console.log('Friend request received:', data);
        updateFriendStatus(data.senderId, 'request', { 
            id: data.id, 
            senderId: data.senderId, 
            senderName: data.senderName, 
            senderEmail: data.senderEmail 
        });
        toastr.info(`${data.senderName} sent you a friend request`);
    });

    $(document).on("friendRequestAccepted", function (e, data) {
        console.log('Friend request accepted:', data);
        updateFriendStatus(data.accepterId, 'friend', { 
            id: data.id, 
            senderId: data.accepterId, 
            senderName: data.accepterName, 
            senderEmail: data.accepterEmail 
        });
        // Удаляем запись из localStorage при получении уведомления о принятии запроса
        const sentRequests = JSON.parse(localStorage.getItem('sentFriendRequests') || '{}');
        delete sentRequests[data.accepterId];
        localStorage.setItem('sentFriendRequests', JSON.stringify(sentRequests));
        toastr.success(`${data.accepterName} accepted your friend request`);
    });

    $(document).on("friendRequestRejected", function (e, data) {
        console.log('Friend request rejected:', data);
        updateFriendStatus(data.rejecterId, 'none', { 
            id: data.id, 
            senderId: data.rejecterId 
        });
        // Удаляем запись из localStorage при получении уведомления об отклонении запроса
        const sentRequests = JSON.parse(localStorage.getItem('sentFriendRequests') || '{}');
        delete sentRequests[data.rejecterId];
        localStorage.setItem('sentFriendRequests', JSON.stringify(sentRequests));
        toastr.info(`${data.rejectorName} rejected your friend request`);
    });

    $(document).on("friendRemoved", function (e, data) {
        console.log('Friend removed:', data);
        updateFriendStatus(data.removerId, 'none', { 
            id: data.id, 
            senderId: data.removerId 
        });
        toastr.info(`${data.removerName} removed you from their friends list`);
    });
}); 