(function () {
    'use strict';

    function init() {
        var form = document.getElementById('post-form');
        if (!form) {
            return;
        }

        form.addEventListener('submit', async function (event) {
            event.preventDefault();

            var titleInput = form.querySelector('[name="Title"]');
            var bodyInput = form.querySelector('[name="Body"]');
            var communityInput = form.querySelector('[name="CommunityId"]');

            var title = titleInput ? titleInput.value.trim() : '';
            var body = bodyInput ? bodyInput.value.trim() : '';
            var communityId = communityInput ? parseInt(communityInput.value, 10) : 0;

            try {
                var response = await fetch('/Post/CreateAjax', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                    },
                    body: JSON.stringify({
                        Title: title,
                        Body: body,
                        CommunityId: communityId
                    })
                });

                var result = null;

                try {
                    result = await response.json();
                } catch {
                    result = null;
                }

                if (!response.ok) {
                    throw new Error(result?.message || 'Failed to create post.');
                }

                var dialog = document.getElementById('post-dialog');
                if (dialog) {
                    dialog.classList.remove('is-open');
                    dialog.setAttribute('aria-hidden', 'true');
                }

                document.body.classList.remove('is-dialog-open');
                form.reset();

                window.location.reload();
            } catch (error) {
                console.error(error);
                alert(error.message || 'Something went wrong creating the post.');
            }
        });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
