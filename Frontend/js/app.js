document.addEventListener('DOMContentLoaded', () => {
    fetchBooks();
    document.getElementById('add-book-form').addEventListener('submit', addBookWithImage);
});

async function fetchBooks() {
    try {    
        const response = await fetch('http://localhost:5201/api/Library')
        if (!response.ok) {
            throw new Error('Error: ' + response.statusText);
        }
        const books = await response.json();
        displayBooks(books);
    } catch(error) {
        console.error('Error fetching books:', error);
    }
}

function displayBooks(books) {
    const bookList = document.getElementById('book-list');
    bookList.innerHTML = '';

    books.forEach(book => {
        const bookCard = document.createElement('div');
        bookCard.classList.add('book-card');

        const bookTitle = document.createElement('h3');
        bookTitle.textContent = book.title;

        const year = document.createElement('h5');
        year.textContent = book.year;

        const description = document.createElement('h4');
        description.textContent = book.description;

        const bookImage = document.createElement('img');
        bookImage.src = 'http://localhost:5201' + book.imagePath; // Display the image
        bookImage.alt = 'Book Cover';

        bookCard.appendChild(bookTitle);
        bookCard.appendChild(year);
        bookCard.appendChild(description);
        bookCard.appendChild(bookImage);

        bookList.appendChild(bookCard);
    });
}

async function addBookWithImage(event) {
    event.preventDefault(); // Prevent default form submission (page reload

    const formData = new FormData();
    formData.append('title', document.getElementById('title').value);
    formData.append('description', document.getElementById('description').value);
    formData.append('year', document.getElementById('year').value);
    formData.append('coverImage', document.getElementById('cover-image').files[0]);

    try {
        const response = await fetch('http://localhost:5201/api/Library/add-book', {
            method: 'POST',
            body: formData // Send form data (including the image)
        });

        if (!response.ok) {
            throw new Error('Error: ' + response.statusText);
        }

        const result = await response.json();
        console.log('Book added successfully:', result);

        // Show success message
        document.getElementById('message').textContent = 'Book added successfully!';

        // Optionally, fetch and display all books again to reflect the new addition
        fetchBooks();

    } catch (error) {
        console.error('Failed to add book:', error);
        document.getElementById('message').textContent = 'Failed to add book: ' + error.message;
    }
}
