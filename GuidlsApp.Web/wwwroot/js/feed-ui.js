(function () {
    'use strict';

    function initVoting() {
        document.addEventListener('click', function (event) {
            var button = event.target.closest('[data-action="vote"]');
            if (!button) {
                return;
            }

            var postCard = button.closest('.post-card');
            if (!postCard) {
                return;
            }

            var countElement = postCard.querySelector('.post-card__vote-count');
            var upButton = postCard.querySelector('.post-card__vote-button--up');
            var downButton = postCard.querySelector('.post-card__vote-button--down');

            var currentVote = upButton.classList.contains('is-voted') ? 1 :
                downButton.classList.contains('is-voted') ? -1 : 0;

            var nextVote = parseInt(button.dataset.value, 10);
            var appliedVote = currentVote === nextVote ? 0 : nextVote;
            var delta = appliedVote - currentVote;
            var currentScore = parseInt(countElement.textContent, 10) || 0;

            countElement.textContent = (currentScore + delta).toString();
            upButton.classList.toggle('is-voted', appliedVote === 1);
            downButton.classList.toggle('is-voted', appliedVote === -1);
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
