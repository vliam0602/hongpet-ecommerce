function formatCurrency(value) {
    return new Intl.NumberFormat('vi-VN',
        {
            style: 'currency',
            currency: 'VND'
        }).format(value);
}

let selectedVariantId = document
    .querySelector('.variant-btn.active')
    .getAttribute('data-variant-id');

// Cập nhật giá của variant ban đầu
let selectedVariantPrice = document
    .querySelector('.variant-btn.active')
    .getAttribute('data-variant-price');

document.getElementById('variant-price').textContent =
    formatCurrency(selectedVariantPrice);

document.querySelectorAll('.variant-btn').forEach(button => {
    button.addEventListener('click', function ()
    {
        document
            .querySelectorAll('.variant-btn')
            .forEach(btn => btn.classList.remove('active'));

        this.classList.add('active');

        // get dynamic data if variant when click
        selectedVariantId = this.getAttribute('data-variant-id');

        selectedVariantPrice = this.getAttribute('data-variant-price');

        const variantName = Array.from(this.querySelectorAll('.attribute-value'))
            .map(span => span.textContent.trim()).join(", ");        

        // update the value
        document.getElementById('variant-price').textContent = formatCurrency(selectedVariantPrice);

        document.getElementById('variantId').value = selectedVariantId;

        document.getElementById('variantName').value = variantName;

        document.getElementById('variantPrice').value = selectedVariantPrice;

    });
});

function updateQuantity(change) {
    const quantityInput = document.getElementById('quantity-input');
    let currentQuantity = parseInt(quantityInput.value);
    currentQuantity = Math.max(1, currentQuantity + change);
    quantityInput.value = currentQuantity;
}
