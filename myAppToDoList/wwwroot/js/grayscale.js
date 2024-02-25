function toggleImage(img) {
    var currentSrc = img.getAttribute('src');
    var newSrc = '';

    if (currentSrc.includes('-bw')) {
        newSrc = currentSrc.replace('-bw.jpg', '.jpg');
    } else {
        newSrc = currentSrc.replace('.jpg', '-bw.jpg');
    }

    img.setAttribute('src', newSrc);

    // Resmin boyutlarını ayarla
    img.style.width = '354px';
    img.style.height = '354px';
}
