(function () {
    'use strict';

    function init() {
        var dialog = document.getElementById('post-dialog');
        if (!dialog) {
            return;
        }

        var titleInput = dialog.querySelector('.composer__title-input');
        var counter = document.getElementById('post-title-counter');

        function openDialog() {
            dialog.classList.add('is-open');
            dialog.setAttribute('aria-hidden', 'false');
            document.body.classList.add('is-dialog-open');

            setTimeout(function () {
                if (titleInput) {
                    titleInput.focus();
                }
            }, 30);
        }

        function closeDialog() {
            dialog.classList.remove('is-open');
            dialog.setAttribute('aria-hidden', 'true');
            document.body.classList.remove('is-dialog-open');
        }

        document.addEventListener('click', function (event) {
            var openButton = event.target.closest('[data-action="open-create-modal"]');
            if (openButton) {
                openDialog();
                return;
            }

            var closeButton = event.target.closest('[data-action="close-create-modal"]');
            if (closeButton) {
                closeDialog();
            }
        });

        document.addEventListener('keydown', function (event) {
            if (event.key === 'Escape' && dialog.classList.contains('is-open')) {
                closeDialog();
            }
        });

        if (titleInput && counter) {
            function updateCounter() {
                counter.textContent = titleInput.value.length.toString();
            }

            titleInput.addEventListener('input', updateCounter);
            updateCounter();
        }
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
