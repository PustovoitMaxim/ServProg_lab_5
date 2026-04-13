// wwwroot/js/site.js
// Site-wide JavaScript functionality

// Format relative time
function formatRelativeTime(dateString) {
    const date = new Date(dateString);
    const now = new Date();
    const diffInSeconds = Math.floor((now - date) / 1000);
    
    if (diffInSeconds < 60) {
        return 'just now';
    }
    
    const diffInMinutes = Math.floor(diffInSeconds / 60);
    if (diffInMinutes < 60) {
        return `${diffInMinutes} minute${diffInMinutes > 1 ? 's' : ''} ago`;
    }
    
    const diffInHours = Math.floor(diffInMinutes / 60);
    if (diffInHours < 24) {
        return `${diffInHours} hour${diffInHours > 1 ? 's' : ''} ago`;
    }
    
    const diffInDays = Math.floor(diffInHours / 24);
    if (diffInDays < 7) {
        return `${diffInDays} day${diffInDays > 1 ? 's' : ''} ago`;
    }
    
    return date.toLocaleDateString();
}

// Lazy load images
document.addEventListener('DOMContentLoaded', function() {
    const imageObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                img.src = img.dataset.src;
                img.classList.add('loaded');
                observer.unobserve(img);
            }
        });
    });
    
    document.querySelectorAll('img[data-src]').forEach(img => {
        imageObserver.observe(img);
    });
});

// Handle like button clicks (demo)
document.querySelectorAll('.like-btn').forEach(btn => {
    btn.addEventListener('click', async function() {
        const postId = this.dataset.postId;
        const likeCountSpan = this.querySelector('.like-count');
        let currentLikes = parseInt(likeCountSpan.textContent);
        
        // Optimistic update
        likeCountSpan.textContent = currentLikes + 1;
        this.style.color = '#dc3545';
        
        // Here you would make an API call to save the like
        // await fetch(`/api/posts/${postId}/like`, { method: 'POST' });
    });
});

// Image lightbox
function openLightbox(imageUrl) {
    const modal = document.createElement('div');
    modal.className = 'lightbox-modal';
    modal.innerHTML = `
        <div class="lightbox-content">
            <img src="${imageUrl}" alt="Full size image">
            <button class="lightbox-close">&times;</button>
        </div>
    `;
    
    modal.addEventListener('click', (e) => {
        if (e.target === modal || e.target.className === 'lightbox-close') {
            modal.remove();
        }
    });
    
    document.body.appendChild(modal);
}

// Add lightbox styles dynamically
const lightboxStyles = `
    .lightbox-modal {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0,0,0,0.9);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 9999;
        cursor: pointer;
    }
    .lightbox-content {
        max-width: 90%;
        max-height: 90%;
        position: relative;
    }
    .lightbox-content img {
        max-width: 100%;
        max-height: 90vh;
        object-fit: contain;
    }
    .lightbox-close {
        position: absolute;
        top: -40px;
        right: 0;
        background: none;
        border: none;
        color: white;
        font-size: 30px;
        cursor: pointer;
    }
`;

const styleSheet = document.createElement("style");
styleSheet.textContent = lightboxStyles;
document.head.appendChild(styleSheet);

// Add click handlers for images
document.addEventListener('click', (e) => {
    if (e.target.tagName === 'IMG' && e.target.classList.contains('post-image')) {
        openLightbox(e.target.src);
    }
});

// Smooth scroll to top
function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
}

// Add scroll to top button
const scrollBtn = document.createElement('button');
scrollBtn.innerHTML = '↑';
scrollBtn.className = 'scroll-top-btn';
scrollBtn.onclick = scrollToTop;
document.body.appendChild(scrollBtn);

const scrollBtnStyles = `
    .scroll-top-btn {
        position: fixed;
        bottom: 20px;
        right: 20px;
        width: 40px;
        height: 40px;
        border-radius: 50%;
        background: #4a90e2;
        color: white;
        border: none;
        cursor: pointer;
        display: none;
        font-size: 20px;
        z-index: 1000;
    }
    .scroll-top-btn:hover {
        background: #2c5aa0;
    }
`;

const scrollBtnStyleSheet = document.createElement("style");
scrollBtnStyleSheet.textContent = scrollBtnStyles;
document.head.appendChild(scrollBtnStyleSheet);

window.addEventListener('scroll', () => {
    if (window.scrollY > 300) {
        scrollBtn.style.display = 'block';
    } else {
        scrollBtn.style.display = 'none';
    }
});

// Console log for debugging
console.log('SocialApp initialized successfully');