<script>
    window.showToast = function (message, type = 'info') {
        const existingToast = document.querySelector('.custom-toast');
        if (existingToast) {
            existingToast.remove();
        }

        const toast = document.createElement('div');
        toast.className = `custom-toast alert alert-${getBootstrapType(type)} alert-dismissible fade show`;
        toast.style.cssText = `
    position: fixed;
    top: 20px;
    right: 20px;
    z-index: 9999;
    min-width: 300px;
    box-shadow: 0 4px 8px rgba(0,0,0,0.1);
    `;

        toast.innerHTML = `
    <div class="d-flex align-items-center">
        <i class="bi ${getIcon(type)} me-2"></i>
        <span>${message}</span>
        <button type="button" class="btn-close ms-auto" onclick="this.parentElement.parentElement.remove()"></button>
    </div>
    `;

        document.body.appendChild(toast);
        
        setTimeout(() => {
            if (toast.parentNode) {
                toast.classList.remove('show');
                setTimeout(() => {
                    if (toast.parentNode) {
                        toast.remove();
                    }
                }, 150);
            }
        }, 3000);
    };

function getBootstrapType(type) {
    switch (type) {
    case 'success': return 'success';
    case 'error': return 'danger';
    case 'warning': return 'warning';
    case 'info':
    default: return 'info';
    }
}

function getIcon(type) {
    switch (type) {
    case 'success': return 'bi-check-circle-fill';
    case 'error': return 'bi-exclamation-triangle-fill';
    case 'warning': return 'bi-exclamation-triangle-fill';
    case 'info':
    default: return 'bi-info-circle-fill';
    }
}
</script>