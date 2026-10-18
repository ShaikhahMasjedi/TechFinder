 

    const carousel = document.querySelector(".carousel");
    const prevButton = document.getElementById("prev");
    const nextButton = document.getElementById("next");
    const scrollStep = 160 * 5;

    nextButton.addEventListener("click", () => {
        carousel.scrollBy({ left: scrollStep, behavior: "smooth" });
    });

    prevButton.addEventListener("click", () => {
        carousel.scrollBy({ left: -scrollStep, behavior: "smooth" });
    });



    function openCategoryProducts(categoryId) {
        // «· Õﬁﬁ „‰ ﬁÌ„… categoryId Ê ÊÃÌÂ «·„” Œœ„ ≈·Ï «·—«»ÿ «·’ÕÌÕ
        window.location.href = '/Categories/Details?categoryId=' + categoryId;
    }


    function showSpinner() {
        document.getElementById("spinner").classList.remove("d-none");
        }



    document.addEventListener("DOMContentLoaded", function () {
        // «·Õ’Ê· ⁄·Ï ﬂ· «·’Ê— ›Ì «·‹ carousel
        var images = document.querySelectorAll('.carousel-items img');

        images.forEach(function (image) {
            // ⁄‰œ„« Ì „ «·‰ﬁ— ⁄·Ï ’Ê—… ›∆…
            image.addEventListener('click', function () {
                var categoryId = image.getAttribute('data-id');
                openCategoryProducts(categoryId); //  „—Ì— «·‹ categoryId
            });
        });
    });



function toggleFavorite(event) {
    const heartIcon = event.target;  // «” Âœ«› «·√ÌﬁÊ‰… «· Ì  „ «·‰ﬁ— ⁄·ÌÂ«
    heartIcon.classList.toggle('liked');  //  »œÌ· «·ﬂ·«” "liked" »Ì‰ «·≈÷«›… Ê«·≈“«·…
    const form = heartIcon.closest('form');  // «·⁄ÀÊ— ⁄·Ï «·›Ê—„ «·„— »ÿ
}













