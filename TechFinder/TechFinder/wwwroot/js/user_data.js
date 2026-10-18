// تفعيل التبديل بين الأقسام
document.querySelectorAll('.tab-button').forEach(button => {
    button.addEventListener('click', function() {
        // إزالة النشاط من جميع الأزرار
        document.querySelectorAll('.tab-button').forEach(btn => btn.classList.remove('active'));
        // إضافة النشاط للزر المحدد
        this.classList.add('active');
        // إخفاء جميع الأقسام
        document.querySelectorAll('.section').forEach(section => section.classList.remove('active'));
        // إظهار القسم المحدد
        const target = this.getAttribute('data-target');
        document.querySelector(`#${target}`).classList.add('active');
    });
});
