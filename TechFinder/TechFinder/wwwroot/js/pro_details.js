// ÇáÊäÞá Èíä ÇáÕæÑ
function changeImage(imageUrl) {
    document.getElementById('main-image').src = imageUrl;
}

//ãÏÉ ÚÑÖ ÇáãáÇÍÙÉ
const alerts = document.querySelectorAll('.alert');
alerts.forEach(alert => {
    setTimeout(() => {
        alert.classList.add('hide');
        setTimeout(() => {
            alert.remove();
        }, 500);
    }, 3000);
});