(function () {
    'use strict';

    function showToast(message) {
        var existing = document.getElementById('app-toast');
        if (existing) {
            existing.remove();
        }

        var toast = document.createElement('div');
        toast.id = 'app-toast';
        toast.textContent = message;

        Object.assign(toast.style, {
            position: 'fixed',
            bottom: '24px',
            left: '50%',
            transform: 'translateX(-50%)',
            background: '#1a2a4a',
            color: '#fff',
            padding: '10px 18px',
            borderRadius: '10px',
            fontSize: '13px',
            fontWeight: '600',
            zIndex: '9999',
            opacity: '0',
            transition: 'opacity 180ms ease'
        });

        document.body.appendChild(toast);

        requestAnimationFrame(function () {
            toast.style.opacity = '1';
        });

        setTimeout(function () {
            toast.style.opacity = '0';
            setTimeout(function () {
                toast.remove();
            }, 180);
        }, 1800);
    }

    window.appUi = window.appUi || {};
    window.appUi.showToast = showToast;
})();
