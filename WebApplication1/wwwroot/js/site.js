// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


//Kategori seçim fonsiyonu
document.addEventListener("DOMContentLoaded", function () {
    const checkboxes = document.querySelectorAll(".category-checkbox");
    const selectedCategoriesDiv = document.getElementById("selectedCategories");

    // Kategori seçimlerinin güncellenmesi
    function updateSelectedCategories() {
        selectedCategoriesDiv.innerHTML = ""; // Seçilen kategoriler div'ini temizle

        let selectedOptions = Array.from(checkboxes).filter(checkbox => checkbox.checked);

        if (selectedOptions.length > 3) {
            // 3'ten fazla seçim yapılırsa, uyarı göster
            alert("En fazla 3 kategori seçebilirsiniz.");
            return;
        }

        // Seçilen kategori sayısına göre, disabled kategorileri güncelle
        checkboxes.forEach(checkbox => {
            if (selectedOptions.length === 3 && !checkbox.checked) {
                checkbox.disabled = true; // 3 kategori seçildiyse, diğerlerini disable et
            } else {
                checkbox.disabled = false; // Seçim yapılmadıkça, checkboxlar aktif kalır
            }
        });

        if (selectedOptions.length === 0) {
            selectedCategoriesDiv.innerHTML = `<span class="text-muted">Henüz kategori seçilmedi.</span>`;
            return;
        }

        selectedOptions.forEach(option => {
            let categoryBadge = document.createElement("span");
            categoryBadge.className = "badge bg-primary m-1 p-2";
            categoryBadge.textContent = option.nextElementSibling.textContent;

            let removeBtn = document.createElement("button");
            removeBtn.innerHTML = " x";
            removeBtn.className = "btn btn-sm btn-danger ms-2";
            removeBtn.style.border = "none";
            removeBtn.style.cursor = "pointer";

            removeBtn.addEventListener("click", function () {
                option.checked = false; // Seçimi kaldır
                updateSelectedCategories(); // Listeyi güncelle
            });

            categoryBadge.appendChild(removeBtn);
            selectedCategoriesDiv.appendChild(categoryBadge);
        });
    }

    checkboxes.forEach(checkbox => {
        checkbox.addEventListener("change", updateSelectedCategories);
    });
});





