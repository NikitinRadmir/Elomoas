document.addEventListener('DOMContentLoaded', function() {
    loadChatUsers();
});

async function loadChatUsers() {
    try {
        const response = await fetch('/Messanger/GetChatUsers');
        if (!response.ok) {
            throw new Error('Failed to load chat users');
        }
        
        const users = await response.json();
        const chatUsersList = document.getElementById('chat-users-list');
        
        users.forEach(user => {
            const li = createChatUserElement(user);
            chatUsersList.appendChild(li);
        });
    } catch (error) {
        console.error('Error loading chat users:', error);
    }
}

function createChatUserElement(user) {
    const li = document.createElement('li');
    li.className = 'bg-transparent list-group-item no-icon pl-0';
    
    li.innerHTML = `
        <figure class="avatar float-left mb-0 mr-3">
            <img src="${user.img || '/images/user-12.png'}" alt="image" class="w45">
        </figure>
        <h3 class="fw-700 mb-0 mt-1">
            <a class="font-xsss text-grey-900 text-dark d-block" href="#" data-user-id="${user.id}">${user.name}</a>
        </h3>
        <span class="d-block">${user.lastMessage || ''}</span>
        ${user.unreadCount ? `<span class="badge badge-primary text-white badge-pill">${user.unreadCount}</span>` : ''}
        ${user.isTyping ? `
        <div class="snippet float-right" data-title=".dot-typing">
            <div class="stage">
                <div class="dot-typing"></div>
            </div>
        </div>` : ''}
    `;

    li.querySelector('a').addEventListener('click', (e) => {
        e.preventDefault();
        openChat(user.id);
    });

    return li;
}

function openChat(userId) {
    // Здесь будет логика открытия чата с пользователем
    console.log('Opening chat with user:', userId);
} 