
function previewImage(event, previewId) {
    const input = event.target;
    const preview = document.getElementById(previewId);

    if (input.files && input.files[0]) {
        const reader = new FileReader();

        reader.onload = function(e) {
            preview.src = e.target.result;
            preview.style.display = 'block';
        };

        reader.readAsDataURL(input.files[0]);
    }
}

document.getElementById('mainImage').addEventListener('change', function(event) {
    previewImage(event, 'previewMainImage');

});

document.getElementById('extraImage1').addEventListener('change', function(event) {
    previewImage(event, 'previewExtraImage1');
});

document.getElementById('extraImage2').addEventListener('change', function(event) {
    previewImage(event, 'previewExtraImage2');
});
