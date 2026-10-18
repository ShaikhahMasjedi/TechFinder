function openImage(imgSrc) { window.open(imgSrc, "_blank"); }


function toggleFavorite(event) {
    const heartIcon = event.target;  // استهداف الأيقونة التي تم النقر عليها
    heartIcon.classList.toggle('liked');  // تبديل الكلاس "liked" بين الإضافة والإزالة
    const form = heartIcon.closest('form');  // العثور على الفورم المرتبط
}



        function showSpinner() {
            document.getElementById("spinner").classList.remove("d-none");
        }
   