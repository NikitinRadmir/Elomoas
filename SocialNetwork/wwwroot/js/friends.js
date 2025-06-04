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
        const actions = {
            'pending': `
                <a href="/Users/UserPage/${userId}" class="mt-0 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-success font-xsssss fw-700 ls-lg text-white">
                    <i class="feather-user-check font-xss"></i> PROFILE
                </a>
                <a href="#" class="mt-2 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-warning font-xsssss fw-700 ls-lg text-white">
                    <i class="feather-clock font-xss"></i> PENDING
                </a>`,
            'friend': `
                <a href="/Users/UserPage/${userId}" class="mt-0 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-success font-xsssss fw-700 ls-lg text-white">
                    <i class="feather-user-check font-xss"></i> PROFILE
                </a>
                <button class="mt-2 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-danger font-xsssss fw-700 ls-lg text-white remove-friend-btn" data-user-id="${userId}">
                    <i class="feather-user-minus font-xss"></i> REMOVE FRIEND
                </button>`,
            'none': `
                <a href="/Users/UserPage/${userId}" class="mt-0 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-success font-xsssss fw-700 ls-lg text-white">
                    <i class="feather-user-check font-xss"></i> PROFILE
                </a>
                <button class="mt-2 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-primary font-xsssss fw-700 ls-lg text-white add-friend-btn" data-user-id="${userId}">
                    <i class="feather-user-plus font-xss"></i> ADD FRIEND
                </button>`,
            'request': `
                <a href="/Users/UserPage/${userId}" class="mt-0 btn pt-2 pb-2 ps-3 pe-3 lh-24 ms-1 ls-3 rounded-xl bg-success font-xsssss fw-700 ls-lg text-white">
                    <i class="feather-user-check font-xss"></i> PROFILE
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
                } else {
                    const actionsContainer = element.find('.friend-actions');
                    actionsContainer.html(actionHtml);
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
                            newStatus = 'pending';
                            break;
                        case 'accept':
                            newStatus = 'friend';
                            break;
                        case 'reject':
                        case 'remove':
                            newStatus = 'none';
                            break;
                    }
                    updateFriendStatus(userId, newStatus);
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