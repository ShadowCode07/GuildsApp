/* =========================================================
   Guildle — app.js
   Vanilla JS — no framework dependencies
   ========================================================= */

(function () {
  'use strict';

  /* ── Voting ─────────────────────────────────────────────── */
  function initVoting() {
    document.addEventListener('click', function (e) {
      const btn = e.target.closest('[data-action="vote"]');
      if (!btn) return;

      const post = btn.closest('.post');
      if (!post) return;

      const postId = post.dataset.postId;
      const value  = parseInt(btn.dataset.value, 10);
      const countEl = post.querySelector('.post__vote-count');
      const upBtn   = post.querySelector('.post__vote-btn--up');
      const downBtn = post.querySelector('.post__vote-btn--down');

      const currentVote = upBtn.classList.contains('is-voted') ? 1
                        : downBtn.classList.contains('is-voted') ? -1
                        : 0;

      const newVote = currentVote === value ? 0 : value;
      const delta   = newVote - currentVote;
      const current = parseInt(countEl.textContent, 10);

      countEl.textContent = current + delta;
      upBtn.classList.toggle('is-voted',   newVote === 1);
      downBtn.classList.toggle('is-voted', newVote === -1);

      fetch(`/api/posts/${postId}/vote`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ value: newVote })
      }).catch(function () {
        countEl.textContent = current;
        upBtn.classList.toggle('is-voted',   currentVote === 1);
        downBtn.classList.toggle('is-voted', currentVote === -1);
      });
    });
  }

  /* ── Share ───────────────────────────────────────────────── */
  function initShare() {
    document.addEventListener('click', function (e) {
      const btn = e.target.closest('[data-action="share"]');
      if (!btn) return;

      const url = window.location.origin + btn.dataset.url;

      if (navigator.share) {
        navigator.share({ url: url }).catch(function () {});
      } else if (navigator.clipboard) {
        navigator.clipboard.writeText(url).then(function () {
          showToast('Link copied!');
        });
      }
    });
  }

  /* ── Save post ───────────────────────────────────────────── */
  function initSave() {
    document.addEventListener('click', function (e) {
      const btn = e.target.closest('[data-action="save"]');
      if (!btn) return;

      const postId = btn.dataset.postId;
      const saved  = btn.dataset.saved === 'true';

      btn.textContent = saved ? 'Save' : 'Saved';
      btn.dataset.saved = (!saved).toString();

      fetch(`/api/posts/${postId}/save`, {
        method: saved ? 'DELETE' : 'POST'
      }).catch(function () {
        btn.textContent = saved ? 'Saved' : 'Save';
        btn.dataset.saved = saved.toString();
      });
    });
  }

  /* ── Sort pills (no page reload — updates URL param) ─────── */
  function initSort() {
    document.querySelectorAll('[data-action="sort"]').forEach(function (btn) {
      btn.addEventListener('click', function () {
        const sort = btn.dataset.sort;
        const url  = new URL(window.location.href);
        url.searchParams.set('sort', sort);
        window.location.href = url.toString();
      });
    });
  }

  /* ── Toast notification ──────────────────────────────────── */
  function showToast(message) {
    const existing = document.getElementById('guildle-toast');
    if (existing) existing.remove();

    const toast = document.createElement('div');
    toast.id = 'guildle-toast';
    toast.textContent = message;
    Object.assign(toast.style, {
      position:     'fixed',
      bottom:       '24px',
      left:         '50%',
      transform:    'translateX(-50%)',
      background:   '#1a2a4a',
      color:        '#fff',
      padding:      '8px 18px',
      borderRadius: '8px',
      fontSize:     '13px',
      fontWeight:   '500',
      zIndex:       '9999',
      opacity:      '0',
      transition:   'opacity 200ms ease'
    });

    document.body.appendChild(toast);
    requestAnimationFrame(function () { toast.style.opacity = '1'; });
    setTimeout(function () {
      toast.style.opacity = '0';
      setTimeout(function () { toast.remove(); }, 200);
    }, 2200);
  }

  /* ── XP bar animation on load ────────────────────────────── */
  function animateXpBar() {
    const bar = document.querySelector('.xp-bar__fill');
    if (!bar) return;
    const target = bar.style.width;
    bar.style.width = '0%';
    requestAnimationFrame(function () {
      requestAnimationFrame(function () {
        bar.style.width = target;
      });
    });
  }

  /* ── Init ────────────────────────────────────────────────── */
  document.addEventListener('DOMContentLoaded', function () {
    initVoting();
    initShare();
    initSave();
    initSort();
    animateXpBar();
  });

})();