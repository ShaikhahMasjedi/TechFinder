


function updateTotal(input) {
    // ÇáÍÕæá Úáì ÇáŞíãÉ ÇáãÏÎáÉ ááßãíÉ æÇáÓÚÑ
    var quantity = input.value;
    var price = input.getAttribute('data-price');
    var totalElement = input.closest('tr').querySelector('.total-price');

    // ÍÓÇÈ ÇáÓÚÑ ÇáÅÌãÇáí ÈäÇÁğ Úáì ÇáßãíÉ
    var total = quantity * price;
    totalElement.innerText = total.toFixed(2); // ÚÑÖ ÇáÊæÊá ÈÔßá ÕÍíÍ

    // ÍÓÇÈ ÇáãÌãæÚ Çáßáí áÌãíÚ ÇáÚäÇÕÑ İí ÇáÓáÉ
    updateCartTotal();
}

function updateCartTotal() {
    var totalAmount = 0;

    // ÌãÚ ÇáãÌãæÚ Çáßáí ãä ÌãíÚ ÇáÕİæİ
    var totalPriceElements = document.querySelectorAll('.total-price');
    totalPriceElements.forEach(function (element) {
        totalAmount += parseFloat(element.innerText) || 0;
    });

    // ÚÑÖ ÇáãÌãæÚ Çáßáí İí ÇáÚäÕÑ ÇáãÎÕÕ áĞáß
    document.getElementById('totalAmount').innerText = totalAmount.toFixed(2);
}

// ÇÓÊÏÚÇÁ ÇáÏÇáÉ ÚäÏ ÊÍãíá ÇáÕİÍÉ áÊÍÏíË ÇáãÌãæÚ Çáßáí ãÈÇÔÑÉ
window.onload = function () {
    updateCartTotal();
}
















