export class UserStorageService {
    constructor() {
        this.storageKey = 'userInfo';
        this.initializeStorage();
    }

    // Initialize storage if empty
    initializeStorage() {
        if (!localStorage.getItem(this.storageKey)) {
            localStorage.setItem(this.storageKey, JSON.stringify({
                name: '',
                email: '',
                phone: ''
            }));
        }
    }

    // Get all user info
    getUserInfo() {
        const userData = localStorage.getItem(this.storageKey);
        return JSON.parse(userData);
    }

    // Get specific user property
    getUserProperty(property) {
        const userData = this.getUserInfo();
        return userData[property] || null;
    }

    // Set user info (can update individual properties or all at once)
    setUserInfo({ name, email, phone }) {
        const currentData = this.getUserInfo();

        const updatedData = {
            name: name !== undefined ? name : currentData.name,
            email: email !== undefined ? email : currentData.email,
            phone: phone !== undefined ? phone : currentData.phone
        };

        // Validate email if provided
        if (email !== undefined && !this.validateEmail(email)) {
            throw new Error('Invalid email format');
        }

        // Validate phone if provided (basic validation)
        if (phone !== undefined && !this.validatePhone(phone)) {
            throw new Error('Invalid phone number');
        }

        localStorage.setItem(this.storageKey, JSON.stringify(updatedData));
        return updatedData;
    }

    // Clear all user info
    clearUserInfo() {
        localStorage.setItem(this.storageKey, JSON.stringify({
            name: '',
            email: '',
            phone: ''
        }));
    }

    // Email validation helper
    validateEmail(email) {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }

    // Phone validation helper (basic)
    validatePhone(phone) {
        // Simple validation - at least 6 digits
        const re = /^[\d\s\-+()]{6,}$/;
        return re.test(phone);
    }
}