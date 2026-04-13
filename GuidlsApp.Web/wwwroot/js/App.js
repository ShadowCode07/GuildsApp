(function () {
    'use strict';

    function initVoting() {
        document.addEventListener('click', function (e) {
            var btn = e.target.closest('[data-action="vote"]');
            if (!btn) return;

            var post = btn.closest('.post');
            if (!post) return;

            var countEl = post.querySelector('.post__vote-count');
            var upBtn = post.querySelector('.post__vote-btn--up');
            var downBtn = post.querySelector('.post__vote-btn--down');

            var currentVote = upBtn.classList.contains('is-voted') ? 1 :
                downBtn.classList.contains('is-voted') ? -1 : 0;

            var value = parseInt(btn.dataset.value, 10);
            var newVote = currentVote === value ? 0 : value;
            var delta = newVote - currentVote;
            var current = parseInt(countEl.textContent, 10) || 0;

            countEl.textContent = current + delta;
            upBtn.classList.toggle('is-voted', newVote === 1);
            downBtn.classList.toggle('is-voted', newVote === -1);
        });
    }

    function initShare() {
        document.addEventListener('click', function (e) {
            var btn = e.target.closest('[data-action="share"]');
            if (!btn) return;

            var url = window.location.origin + btn.dataset.url;

            if (navigator.share) {
                navigator.share({ url: url }).catch(function () { });
            } else if (navigator.clipboard) {
                navigator.clipboard.writeText(url).then(function () {
                    showToast('Link copied!');
                });
            }
        });
    }

    function initSave() {
        document.addEventListener('click', function (e) {
            var btn = e.target.closest('[data-action="save"]');
            if (!btn) return;

            var saved = btn.dataset.saved === 'true';
            btn.dataset.saved = (!saved).toString();
            btn.textContent = saved ? 'Save' : 'Saved';
        });
    }

    function initSort() {
        document.querySelectorAll('[data-action="sort"]').forEach(function (btn) {
            btn.addEventListener('click', function () {
                document.querySelectorAll('[data-action="sort"]').forEach(function (item) {
                    item.classList.remove('is-active');
                });

                btn.classList.add('is-active');
            });
        });
    }

    function showToast(message) {
        var existing = document.getElementById('guildle-toast');
        if (existing) existing.remove();

        var toast = document.createElement('div');
        toast.id = 'guildle-toast';
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

    document.addEventListener('DOMContentLoaded', function () {
        initVoting();
        initShare();
        initSave();
        initSort();
    });
})();