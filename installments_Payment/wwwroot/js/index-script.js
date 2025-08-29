
// Cart functionality
let cart = [];
let cartTotal = 0;

function addToCart(id, name, price) {
    const existingItem = cart.find(item => item.id === id);
    
    if (existingItem) {
        existingItem.quantity += 1;
    } else {
        cart.push({
            id: id,
            name: name,
            price: price,
            quantity: 1
        });
    }
    
    updateCartUI();
    showCartSidebar();
    
    // Show success message
    showToast(`${name} به سبد خرید اضافه شد`, 'success');
}

function updateCartUI() {
    const cartCount = document.getElementById('cartCount');
    const cartItems = document.getElementById('cartItems');
    const cartTotalElement = document.getElementById('cartTotal');
    
    // Update cart count
    const totalItems = cart.reduce((sum, item) => sum + item.quantity, 0);
    cartCount.textContent = totalItems;
    
    // Update cart total
    cartTotal = cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    cartTotalElement.textContent = cartTotal.toLocaleString() + ' تومان';
    
    // Update cart items
    if (cart.length === 0) {
        cartItems.innerHTML = `
            <div class="text-center text-gray-500 py-12">
                <i class="fas fa-shopping-cart text-4xl mb-4"></i>
                <p>سبد خرید شما خالی است</p>
            </div>
        `;
    } else {
        cartItems.innerHTML = cart.map(item => `
            <div class="flex items-center justify-between p-4 border-b">
                <div class="flex-1">
                    <h4 class="font-semibold text-gray-800">${item.name}</h4>
                    <p class="text-gray-600">${item.price.toLocaleString()} تومان</p>
                </div>
                <div class="flex items-center space-x-2 space-x-reverse">
                    <button onclick="decreaseQuantity(${item.id})" class="w-8 h-8 bg-gray-200 rounded-full flex items-center justify-center">
                        <i class="fas fa-minus text-sm"></i>
                    </button>
                    <span class="font-bold mx-2">${item.quantity}</span>
                    <button onclick="increaseQuantity(${item.id})" class="w-8 h-8 bg-cyan-500 text-white rounded-full flex items-center justify-center">
                        <i class="fas fa-plus text-sm"></i>
                    </button>
                    <button onclick="removeFromCart(${item.id})" class="text-red-500 hover:text-red-700 mr-2">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
            </div>
        `).join('');
    }
}

function increaseQuantity(id) {
    const item = cart.find(item => item.id === id);
    if (item) {
        item.quantity += 1;
        updateCartUI();
    }
}

function decreaseQuantity(id) {
    const item = cart.find(item => item.id === id);
    if (item && item.quantity > 1) {
        item.quantity -= 1;
        updateCartUI();
    }
}

function removeFromCart(id) {
    cart = cart.filter(item => item.id !== id);
    updateCartUI();
}

function showCartSidebar() {
    document.getElementById('cartSidebar').classList.remove('-translate-x-full');
    document.getElementById('cartOverlay').classList.remove('hidden');
}

function hideCartSidebar() {
    document.getElementById('cartSidebar').classList.add('-translate-x-full');
    document.getElementById('cartOverlay').classList.add('hidden');
}

// Toast notification
function showToast(message, type = 'info') {
    const toast = document.createElement('div');
    toast.className = `fixed top-4 right-4 z-50 p-4 rounded-lg text-white font-semibold transition-all duration-300 transform translate-x-full`;
    
    if (type === 'success') {
        toast.classList.add('bg-green-500');
        message = `✅ ${message}`;
    } else if (type === 'error') {
        toast.classList.add('bg-red-500');
        message = `❌ ${message}`;
    } else {
        toast.classList.add('bg-blue-500');
        message = `ℹ️ ${message}`;
    }
    
    toast.textContent = message;
    document.body.appendChild(toast);
    
    setTimeout(() => {
        toast.classList.remove('translate-x-full');
    }, 100);
    
    setTimeout(() => {
        toast.classList.add('translate-x-full');
        setTimeout(() => {
            document.body.removeChild(toast);
        }, 300);
    }, 3000);
}

// Search functionality
function setupSearch() {
    const searchInput = document.getElementById('searchInput');
    const searchResults = document.getElementById('searchResults');
    
    if (!searchInput) return;
    
    const medicines = [
        { name: 'استامینوفن', category: 'ضد درد', price: '15,000' },
        { name: 'ایبوپروفن', category: 'ضد التهاب', price: '18,000' },
        { name: 'سرماخوردگی', category: 'شربت', price: '25,000' },
        { name: 'ویتامین سی', category: 'مکمل', price: '22,000' },
        { name: 'آنتی‌بیوتیک', category: 'قرص', price: '35,000' }
    ];
    
    searchInput.addEventListener('input', function() {
        const query = this.value.trim();
        
        if (query.length < 2) {
            searchResults.classList.remove('show');
            return;
        }
        
        const filteredResults = medicines.filter(medicine => 
            medicine.name.includes(query) || 
            medicine.category.includes(query)
        );
        
        if (filteredResults.length > 0) {
            searchResults.innerHTML = filteredResults.map(medicine => `
                <div class="p-4 hover:bg-gray-50 cursor-pointer border-b">
                    <div class="flex items-center justify-between">
                        <div>
                            <h4 class="font-semibold text-gray-800">${medicine.name}</h4>
                            <p class="text-sm text-gray-600">${medicine.category}</p>
                        </div>
                        <div class="text-cyan-600 font-bold">${medicine.price} تومان</div>
                    </div>
                </div>
            `).join('');
            searchResults.classList.add('show');
        } else {
            searchResults.innerHTML = `
                <div class="p-4 text-center text-gray-500">
                    <i class="fas fa-search text-2xl mb-2"></i>
                    <p>محصولی با این نام یافت نشد</p>
                </div>
            `;
            searchResults.classList.add('show');
        }
    });
    
    // Close search results when clicking outside
    document.addEventListener('click', function(e) {
        if (!searchInput.contains(e.target) && !searchResults.contains(e.target)) {
            searchResults.classList.remove('show');
        }
    });
}

// Prescription upload
function setupPrescriptionUpload() {
    const uploadArea = document.getElementById('prescriptionUpload');
    const fileInput = document.getElementById('prescriptionFile');
    
    if (!uploadArea || !fileInput) return;
    
    uploadArea.addEventListener('click', () => {
        fileInput.click();
    });
    
    uploadArea.addEventListener('dragover', (e) => {
        e.preventDefault();
        uploadArea.classList.add('dragover');
    });
    
    uploadArea.addEventListener('dragleave', () => {
        uploadArea.classList.remove('dragover');
    });
    
    uploadArea.addEventListener('drop', (e) => {
        e.preventDefault();
        uploadArea.classList.remove('dragover');
        
        const files = e.dataTransfer.files;
        if (files.length > 0) {
            handleFileUpload(files[0]);
        }
    });
    
    fileInput.addEventListener('change', (e) => {
        if (e.target.files.length > 0) {
            handleFileUpload(e.target.files[0]);
        }
    });
}

function handleFileUpload(file) {
    if (file.size > 5 * 1024 * 1024) {
        showToast('حجم فایل نباید بیشتر از 5 مگابایت باشد', 'error');
        return;
    }
    
    const allowedTypes = ['image/jpeg', 'image/png', 'application/pdf'];
    if (!allowedTypes.includes(file.type)) {
        showToast('فقط فایل‌های JPG، PNG و PDF مجاز هستند', 'error');
        return;
    }
    
    // Show loading
    const loadingModal = document.getElementById('loadingModal');
    loadingModal.classList.remove('hidden');
    
    // Simulate upload
    setTimeout(() => {
        loadingModal.classList.add('hidden');
        showToast('نسخه با موفقیت آپلود شد و در حال بررسی است', 'success');
        
        // Update upload area
        const uploadArea = document.getElementById('prescriptionUpload');
        uploadArea.innerHTML = `
            <div class="text-center">
                <i class="fas fa-check-circle text-4xl text-green-500 mb-4"></i>
                <h3 class="text-lg font-bold text-gray-800 mb-2">نسخه آپلود شد</h3>
                <p class="text-gray-600 mb-4">${file.name}</p>
                <button onclick="location.reload()" class="text-cyan-600 hover:text-cyan-800 font-semibold">
                    آپلود نسخه جدید
                </button>
            </div>
        `;
    }, 2000);
}

// Mobile menu
function setupMobileMenu() {
    const mobileMenuBtn = document.getElementById('mobileMenuBtn');
    const mobileMenu = document.getElementById('mobileMenu');
    
    if (!mobileMenuBtn || !mobileMenu) return;
    
    mobileMenuBtn.addEventListener('click', () => {
        mobileMenu.classList.toggle('hidden');
    });
}

// Event listeners
document.addEventListener('DOMContentLoaded', function() {
    // Setup components
    setupSearch();
    setupPrescriptionUpload();
    setupMobileMenu();
    
    // Cart functionality
    document.getElementById('cartBtn').addEventListener('click', showCartSidebar);
    document.getElementById('closeCart').addEventListener('click', hideCartSidebar);
    document.getElementById('cartOverlay').addEventListener('click', hideCartSidebar);
    
    // Smooth scrolling
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
    
    // Initialize cart
    updateCartUI();
    
    console.log('🏥 Online Pharmacy website loaded successfully!');
});
