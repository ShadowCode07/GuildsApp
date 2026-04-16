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

    function initCreatePostModal() {
        var modal = document.getElementById('create-post-modal');
        if (!modal) return;

        var titleInput = modal.querySelector('.post-composer__title-input');
        var countEl = document.getElementById('post-title-count');

        function openModal() {
            modal.classList.add('is-open');
            modal.setAttribute('aria-hidden', 'false');
            document.body.classList.add('modal-open');

            setTimeout(function () {
                if (titleInput) titleInput.focus();
            }, 30);
        }

        function closeModal() {
            modal.classList.remove('is-open');
            modal.setAttribute('aria-hidden', 'true');
            document.body.classList.remove('modal-open');
        }

        document.addEventListener('click', function (e) {
            var openBtn = e.target.closest('[data-action="open-create-modal"]');
            if (openBtn) {
                openModal();
                return;
            }

            var closeBtn = e.target.closest('[data-action="close-create-modal"]');
            if (closeBtn) {
                closeModal();
            }
        });

        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && modal.classList.contains('is-open')) {
                closeModal();
            }
        });

        if (titleInput && countEl) {
            function updateCount() {
                countEl.textContent = titleInput.value.length.toString();
            }

            titleInput.addEventListener('input', updateCount);
            updateCount();
        }
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
        initCreatePostModal();
        initCreatePostSubmit();
    });
})();

function initCreatePostSubmit() {
    var form = document.getElementById('create-post-form');
    if (!form) return;

    form.addEventListener('submit', async function (e) {
        e.preventDefault();

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

            var modal = document.getElementById('create-post-modal');
            if (modal) {
                modal.classList.remove('is-open');
                modal.setAttribute('aria-hidden', 'true');
            }

            document.body.classList.remove('modal-open');
            form.reset();

            window.location.reload();
        } catch (err) {
            console.error(err);
            alert(err.message || 'Something went wrong creating the post.');
        }
    });
}