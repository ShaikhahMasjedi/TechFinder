// Check if Favorites List is Empty

function checkEmptyState() {
    const favoritesList = document.querySelector('.favorites-list');
    const emptyState = document.querySelector('.empty-state');
    if (favoritesList.children.length === 1) { // إذا لم يتبق سوى رسالة الفراغ
        emptyState.style.display = 'flex';
    }
}

// Initial Check
checkEmptyState();