
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

        selectedVariantId = this.getAttribute('data-variant-id');
        selectedVariantPrice = this.getAttribute('data-variant-price');

        // Cập nhật giá hiển thị
        document.getElementById('variant-price').textContent =
            formatCurrency(selectedVariantPrice);
    });
});

function updateQuantity(change) {
    const quantityInput = document.getElementById('quantity-input');
    let currentQuantity = parseInt(quantityInput.value);
    currentQuantity = Math.max(1, currentQuantity + change);
    quantityInput.value = currentQuantity;
}

function addToCart() {
    const quantity = document.getElementById('quantity-input').value;

    // Lấy URL từ thuộc tính data-url
    const url = document
        .getElementById('add-to-cart-btn')
        .getAttribute('data-url');

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify({ variantId: selectedVariantId, quantity: quantity })
    }).then(response => {
        if (response.ok) {
            alert('Đã thêm sản phẩm vào giỏ hàng!');
        } else {
            alert('Thêm sản phẩm vào giỏ hàng thất bại!');
        }
    });
}
