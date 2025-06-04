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
        const isUserPage = window.location.pathname.includes('/Users/UserPage/');
        const actions = {
            'pending': isUserPage ? `
                <a href="#" class="mt-2 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-warning font-xsssss fw-700 ls-lg text-white">
                    <i class="feather-clock font-xss"></i> PENDING
                </a>` : `
                <a href="/Users/UserPage/${userId}" class="mt-0 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white">
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
                <a href="/Users/UserPage/${userId}" class="mt-0 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white">
                    <i class="feather-user font-xss"></i> PROFILE
                </a>
                <button class="mt-2 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-danger font-xsssss fw-700 ls-lg text-white remove-friend-btn" data-user-id="${userId}">
                    <i class="feather-user-minus font-xss"></i> REMOVE FRIEND
                </button>`,
            'none': isUserPage ? `
                <a href="#" class="mt-2 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white add-friend-btn" data-user-id="${userId}">
                    <i class="feather-user-plus font-xss"></i> ADD FRIEND
                </a>` : `
                <a href="/Users/UserPage/${userId}" class="mt-0 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white">
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
                <a href="/Users/UserPage/${userId}" class="mt-0 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white">
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

        // If we can't find the user's card and this is a new friend request
        if (elements.length === 0 && status === 'request' && senderData) {
            // Get the requests container
            const requestsSection = $('.pending-requests-container');
            if (requestsSection.length) {
                // Create the new user card
                const userCard = $(`
                    <div class="col-xl-4 col-lg-6 col-md-6 user-item" data-user-id="${senderData.senderId}">
                        <div class="card mb-4 d-block w-100 shadow-xss rounded-lg p-4 border-0 text-center user-card">
                            <a href="/Users/UserPage/${senderData.senderId}" class="ml-auto mr-auto rounded-lg overflow-hidden d-inline-block user-image">
                                <img src="/images/user-12.png" alt="${senderData.senderName}" class="p-0 w100 shadow-xss">
                            </a>
                            <h4 class="fw-700 font-xs mt-3 mb-1">${senderData.senderName}</h4>
                            <p class="fw-600 font-xssss text-grey-500 mt-0 mb-2">${senderData.senderEmail}</p>
                            <div class="clearfix"></div>
                            <div class="friend-actions" data-user-id="${senderData.senderId}">
                                ${actions['request']}
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
        const token = $('input[name="__RequestVerificationToken"]').val();
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
                button.prop('disabled', false);
                if (response.success) {
                    let newStatus = '';
                    switch (action) {
                        case 'add':
                        case 'send':
                            newStatus = 'pending';
                            // Обновляем UI для отправителя, показывая PENDING
                            updateFriendStatus(userId, 'pending');
                            // Сохраняем состояние в localStorage
                            const sentRequests = JSON.parse(localStorage.getItem('sentFriendRequests') || '{}');
                            sentRequests[userId] = true;
                            localStorage.setItem('sentFriendRequests', JSON.stringify(sentRequests));
                            break;
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
            error: function() {
                button.prop('disabled', false);
                toastr.error('There was an error processing your request');
            }
        });
    }

    // При загрузке страницы проверяем отправленные запросы
    function checkSentRequests() {
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

    // Click handlers for friend-related buttons
    $(document).on('click', '.add-friend-btn', function(e) {
        e.preventDefault();
        const userId = $(this).data('user-id');
        handleFriendAction(userId, 'add', $(this));
    });

    $(document).on('click', '.accept-friend-btn', function(e) {
        e.preventDefault();
        const userId = $(this).data('user-id');
        handleFriendAction(userId, 'accept', $(this));
    });

    $(document).on('click', '.reject-friend-btn', function(e) {
        e.preventDefault();
        const userId = $(this).data('user-id');
        handleFriendAction(userId, 'reject', $(this));
    });

    $(document).on('click', '.remove-friend-btn', function(e) {
        e.preventDefault();
        const userId = $(this).data('user-id');
        handleFriendAction(userId, 'remove', $(this));
    });

    // SignalR event handlers
    $(document).on("friendRequest", function (e, data) {
        updateFriendStatus(data.senderId, 'request', data);
        toastr.info(`${data.senderName} sent you a friend request`);
    });

    $(document).on("friendRequestAccepted", function (e, data) {
        updateFriendStatus(data.acceptorId, 'friend');
        // Удаляем запись из localStorage при получении уведомления о принятии запроса
        const sentRequests = JSON.parse(localStorage.getItem('sentFriendRequests') || '{}');
        delete sentRequests[data.acceptorId];
        localStorage.setItem('sentFriendRequests', JSON.stringify(sentRequests));
        toastr.success(`${data.acceptorName} accepted your friend request`);
    });

    $(document).on("friendRequestRejected", function (e, data) {
        updateFriendStatus(data.rejectorId, 'none');
        toastr.info(`${data.rejectorName} rejected your friend request`);
    });

    $(document).on("friendRemoved", function (e, data) {
        updateFriendStatus(data.removerId, 'none');
        toastr.info(`${data.removerName} removed you from their friends list`);
    });
}); 