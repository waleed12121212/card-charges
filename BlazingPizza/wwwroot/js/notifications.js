// Enhanced Notification Functions
window.notificationHelpers = {
    // Toast notification system
    showToast: function(type, title, message, duration = 5000) {
        const toastContainer = document.querySelector('.toast-container') || this.createToastContainer();
        
        const toast = document.createElement('div');
        toast.className = `toast-notification ${type}`;
        toast.innerHTML = `
            <div class="toast-icon">
                ${this.getToastIcon(type)}
                </div>
            <div class="toast-content">
                <div class="toast-title">${title}</div>
                <div class="toast-message">${message}</div>
            </div>
            <button class="toast-close" onclick="this.parentElement.remove()">
                <i class="fas fa-times"></i>
            </button>
        `;

        toastContainer.appendChild(toast);

        // Auto remove after duration
            setTimeout(() => {
            if (toast.parentElement) {
                toast.style.animation = 'slideOut 0.3s ease';
                setTimeout(() => toast.remove(), 300);
                }
            }, duration);

        return toast;
    },

    createToastContainer: function() {
        const container = document.createElement('div');
        container.className = 'toast-container';
        document.body.appendChild(container);
        return container;
    },

    getToastIcon: function(type) {
        const icons = {
            success: '<i class="fas fa-check-circle text-success"></i>',
            error: '<i class="fas fa-exclamation-circle text-danger"></i>',
            warning: '<i class="fas fa-exclamation-triangle text-warning"></i>',
            info: '<i class="fas fa-info-circle text-info"></i>'
        };
        return icons[type] || icons.info;
    },

    // Notification panel animations
    animateNotificationCount: function(element) {
        if (element) {
            element.style.animation = 'bounce 0.6s ease';
            setTimeout(() => {
                element.style.animation = '';
            }, 600);
        }
    },

    // Smooth scroll to notification
    scrollToNotification: function(notificationId) {
        const notification = document.querySelector(`[data-notification-id="${notificationId}"]`);
        if (notification) {
            notification.scrollIntoView({
                behavior: 'smooth',
                block: 'center'
            });
            
            // Highlight the notification
            notification.style.backgroundColor = '#fff3cd';
            setTimeout(() => {
                notification.style.backgroundColor = '';
            }, 2000);
        }
    },

    // Initialize notification panel features
    initializeNotificationPanel: function() {
        console.log('Initializing notification panel...');
        
        // Add keyboard navigation
        document.addEventListener('keydown', (e) => {
            if (e.ctrlKey && e.key === 'n') {
                e.preventDefault();
                const notificationPanel = document.querySelector('.notification-panel');
                if (notificationPanel) {
                    notificationPanel.scrollIntoView({ behavior: 'smooth' });
                }
            }
        });

        // Add intersection observer for lazy loading
        const observerOptions = {
            root: document.querySelector('.notification-list'),
            rootMargin: '0px',
            threshold: 0.1
        };

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('visible');
                }
            });
        }, observerOptions);

        // Observe notification items
        document.querySelectorAll('.notification-item').forEach(item => {
            observer.observe(item);
        });
    },

    // Web Notifications API integration
    requestNotificationPermission: async function() {
        if (!("Notification" in window)) {
            console.log("This browser does not support notifications");
            return false;
        }

        if (Notification.permission === "granted") {
            return true;
        }

        if (Notification.permission !== "denied") {
            const permission = await Notification.requestPermission();
            return permission === "granted";
        }

        return false;
    },

    showBrowserNotification: function(title, message, options = {}) {
        if (Notification.permission === "granted") {
            const notification = new Notification(title, {
                body: message,
                icon: '/img/logo2.png',
                badge: '/img/notification-badge.png',
                tag: 'pizza-notification',
                requireInteraction: false,
                ...options
            });

            notification.onclick = function() {
                window.focus();
                notification.close();
            };

            // Auto close after 5 seconds
            setTimeout(() => notification.close(), 5000);
        }
    },

    // Sound notifications
    playNotificationSound: function(type = 'default') {
        // Create a simple notification sound using Web Audio API
        try {
            const audioContext = new (window.AudioContext || window.webkitAudioContext)();
            const oscillator = audioContext.createOscillator();
            const gainNode = audioContext.createGain();
            
            oscillator.connect(gainNode);
            gainNode.connect(audioContext.destination);
            
            // Different frequencies for different notification types
            const frequencies = {
                default: 440,
                success: 523.25,
                error: 220,
                warning: 349.23,
                info: 261.63
            };
            
            oscillator.frequency.value = frequencies[type] || frequencies.default;
            oscillator.type = 'sine';
            
            gainNode.gain.setValueAtTime(0, audioContext.currentTime);
            gainNode.gain.linearRampToValueAtTime(0.1, audioContext.currentTime + 0.01);
            gainNode.gain.exponentialRampToValueAtTime(0.001, audioContext.currentTime + 0.5);
            
            oscillator.start(audioContext.currentTime);
            oscillator.stop(audioContext.currentTime + 0.5);
        } catch (e) {
            console.log('Could not play notification sound:', e);
        }
    },

    // Badge count management
    updateBadgeCount: function(count) {
        // Update favicon badge (if supported)
        this.updateFaviconBadge(count);
        
        // Update page title
        const baseTitle = 'Blazing Pizza';
        document.title = count > 0 ? `(${count}) ${baseTitle}` : baseTitle;
    },

    updateFaviconBadge: function(count) {
        const canvas = document.createElement('canvas');
        canvas.width = 32;
        canvas.height = 32;
        const ctx = canvas.getContext('2d');

        // Draw the base favicon
        const favicon = document.querySelector('link[rel="icon"]');
        if (favicon) {
            const img = new Image();
            img.onload = function() {
                ctx.drawImage(img, 0, 0, 32, 32);
                
                if (count > 0) {
                    // Draw badge
                    ctx.fillStyle = '#dc3545';
                    ctx.beginPath();
                    ctx.arc(24, 8, 8, 0, 2 * Math.PI);
                    ctx.fill();
                    
                    // Draw count
                    ctx.fillStyle = 'white';
                    ctx.font = 'bold 10px Arial';
                    ctx.textAlign = 'center';
                    const text = count > 99 ? '99+' : count.toString();
                    ctx.fillText(text, 24, 12);
                }
                
                // Update favicon
                const link = document.querySelector('link[rel="icon"]') || document.createElement('link');
                link.type = 'image/png';
                link.rel = 'icon';
                link.href = canvas.toDataURL();
                document.head.appendChild(link);
            };
            img.src = favicon.href;
        }
    }
};

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    window.notificationHelpers.initializeNotificationPanel();
    window.notificationHelpers.requestNotificationPermission();
});

// Global functions for Blazor interop
window.showToast = function(type, title, message) {
    window.notificationHelpers.showToast(type, title, message);
};

window.updateNotificationBadge = function(count) {
    window.notificationHelpers.updateBadgeCount(count);
};

window.playNotificationSound = function(type) {
    window.notificationHelpers.playNotificationSound(type);
};

window.showBrowserNotification = function(title, message, options) {
    window.notificationHelpers.showBrowserNotification(title, message, options);
}; 