(function () {
    'use strict';

    function initVoting() {
        document.addEventListener('click', async function (event) {
            var button = event.target.closest('[data-action="vote"]');
            if (!button) {
                return;
            }

            var post = button.closest('.post-card, .post-thread__post');
            if (!post) {
                return;
            }

            var countElement = post.querySelector('.post-card__vote-count, .post-thread__score');
            var upButton = post.querySelector('.post-card__vote-button--up');
            var downButton = post.querySelector('.post-card__vote-button--down');
            var postId = parseInt(post.dataset.postId, 10);
            var nextVote = parseInt(button.dataset.value, 10);

            if (!postId || !nextVote || !countElement || !upButton || !downButton) {
                return;
            }

            button.disabled = true;

            try {
                var response = await fetch('/Post/VoteAjax', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                    },
                    body: JSON.stringify({
                        postId: postId,
                        value: nextVote
                    })
                });

                var result = null;

                try {
                    result = await response.json();
                } catch {
                    result = null;
                }

                if (response.status === 401 || (response.redirected && response.url.indexOf('/Account/Login') !== -1)) {
                    window.location.href = '/Account/Login';
                    return;
                }

                if (!response.ok || !result || !result.success) {
                    throw new Error(result && result.message ? result.message : 'Failed to save vote.');
                }

                countElement.textContent = result.score.toString();
                upButton.classList.toggle('is-voted', result.currentUserVote === 1);
                downButton.classList.toggle('is-voted', result.currentUserVote === -1);
            } catch (error) {
                console.error(error);

                if (window.appUi && typeof window.appUi.showToast === 'function') {
                    window.appUi.showToast(error.message || 'Unable to save vote.');
                } else {
                    alert(error.message || 'Unable to save vote.');
                }
            } finally {
                button.disabled = false;
            }
        });
    }

    function initShare() {
        document.addEventListener('click', function (event) {
            var button = event.target.closest('[data-action="share"]');
            if (!button) {
                return;
            }

            var url = window.location.origin + button.dataset.url;

            if (navigator.share) {
                navigator.share({ url: url }).catch(function () { });
                return;
            }

            if (navigator.clipboard) {
                navigator.clipboard.writeText(url).then(function () {
                    if (window.appUi && typeof window.appUi.showToast === 'function') {
                        window.appUi.showToast('Link copied!');
                    }
                });
            }
        });
    }

    function initSave() {
        document.addEventListener('click', function (event) {
            var button = event.target.closest('[data-action="save"]');
            if (!button) {
                return;
            }

            var isSaved = button.dataset.saved === 'true';
            button.dataset.saved = (!isSaved).toString();
            button.textContent = isSaved ? 'Save' : 'Saved';
        });
    }

    function initSort() {
        document.querySelectorAll('[data-action="sort"]').forEach(function (button) {
            button.addEventListener('click', function () {
                document.querySelectorAll('[data-action="sort"]').forEach(function (item) {
                    item.classList.remove('is-active');
                });

                button.classList.add('is-active');
            });
        });
    }

    function init() {
        initVoting();
        initShare();
        initSave();
        initSort();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
