// Notification functions for BlazingPizza
window.blazingPizzaNotifications = {
    // Show a toast notification
    showToast: function (title, message, type = 'success', duration = 5000) {
        // Remove existing toasts first
        this.removeExistingToasts();
        
        // Create toast container if it doesn't exist
        let container = document.getElementById('toast-container');
        if (!container) {
            container = document.createElement('div');
            container.id = 'toast-container';
            container.className = 'toast-container';
            container.style.cssText = `
                position: fixed;
                top: 20px;
                right: 20px;
                z-index: 9999;
                max-width: 350px;
            `;
            document.body.appendChild(container);
        }

        // Create toast element
        const toast = document.createElement('div');
        toast.className = `toast show toast-${type}`;
        toast.setAttribute('role', 'alert');
        toast.setAttribute('aria-live', 'assertive');
        toast.setAttribute('aria-atomic', 'true');
        
        // Toast styles
        const typeColors = {
            success: { bg: '#d4edda', border: '#c3e6cb', text: '#155724' },
            error: { bg: '#f8d7da', border: '#f5c6cb', text: '#721c24' },
            warning: { bg: '#fff3cd', border: '#ffeaa7', text: '#856404' },
            info: { bg: '#d1ecf1', border: '#bee5eb', text: '#0c5460' }
        };
        
        const colors = typeColors[type] || typeColors.success;
        
        toast.style.cssText = `
            background-color: ${colors.bg};
            border: 1px solid ${colors.border};
            color: ${colors.text};
            margin-bottom: 10px;
            border-radius: 0.375rem;
            box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
            max-width: 350px;
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
            direction: rtl;
            text-align: right;
        `;

        // Toast content
        toast.innerHTML = `
            <div style="display: flex; align-items: flex-start; padding: 0.75rem;">
                <div style="flex: 1;">
                    <div style="font-weight: 600; margin-bottom: 0.25rem; font-size: 0.875rem;">
                        ${this.escapeHtml(title)}
                    </div>
                    <div style="font-size: 0.875rem; opacity: 0.8;">
                        ${this.escapeHtml(message)}
                    </div>
                </div>
                <button type="button" style="
                    background: none;
                    border: none;
                    font-size: 1.25rem;
                    font-weight: 700;
                    line-height: 1;
                    color: ${colors.text};
                    opacity: 0.5;
                    cursor: pointer;
                    padding: 0;
                    margin-left: 0.75rem;
                " onclick="this.parentElement.parentElement.remove()">×</button>
            </div>
        `;

        // Add to container
        container.appendChild(toast);

        // Auto remove after duration
        if (duration > 0) {
            setTimeout(() => {
                if (toast.parentNode) {
                    toast.style.opacity = '0';
                    toast.style.transition = 'opacity 0.3s ease';
                    setTimeout(() => {
                        if (toast.parentNode) {
                            toast.remove();
                        }
                    }, 300);
                }
            }, duration);
        }

        return toast;
    },

    // Show success notification
    showSuccess: function (title, message, duration = 5000) {
        return this.showToast(title, message, 'success', duration);
    },

    // Show error notification
    showError: function (title, message, duration = 7000) {
        return this.showToast(title, message, 'error', duration);
    },

    // Show warning notification
    showWarning: function (title, message, duration = 6000) {
        return this.showToast(title, message, 'warning', duration);
    },

    // Show info notification
    showInfo: function (title, message, duration = 5000) {
        return this.showToast(title, message, 'info', duration);
    },

    // Remove existing toasts
    removeExistingToasts: function () {
        const existingToasts = document.querySelectorAll('.toast');
        existingToasts.forEach(toast => {
            toast.style.opacity = '0';
            toast.style.transition = 'opacity 0.2s ease';
            setTimeout(() => {
                if (toast.parentNode) {
                    toast.remove();
                }
            }, 200);
        });
    },

    // Escape HTML to prevent XSS
    escapeHtml: function (text) {
        const map = {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            '"': '&quot;',
            "'": '&#039;'
        };
        return text.replace(/[&<>"']/g, function (m) { return map[m]; });
    },

    // Show notification based on type
    showNotification: function (notification) {
        const typeMap = {
            1: 'success', // Purchase
            2: 'success', // CreditTopUp
            3: 'success', // PackagePurchase
            4: 'info',    // System
            5: 'warning', // Warning
            6: 'success'  // Success
        };
        
        const type = typeMap[notification.type] || 'info';
        return this.showToast(notification.title, notification.message, type);
    }
};

// Legacy support for existing alert calls
window.showNotification = function (title, message, type = 'success') {
    window.blazingPizzaNotifications.showToast(title, message, type);
}; 