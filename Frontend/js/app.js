

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
        bookImage.src = 'http://localhost:5201' + book.imagePath;
        bookImage.alt = 'Book Cover';

        const reserveButton = document.createElement('button');
        reserveButton.textContent = 'Reserve';

        reserveButton.addEventListener('click', () => {
            toggleReserveForm();
        });

        const closeButton = document.getElementById('close-form');

        closeButton.addEventListener('click', () => {
            toggleReserveForm();
        });

        document.getElementById("reserve-form").addEventListener("submit", function (event) {
            event.preventDefault();
            
            const bookId = 1; // have to set this up in the backend obviously
            fetchBook(bookId);
            toggleReserveForm();
        });

        bookCard.appendChild(bookTitle);
        bookCard.appendChild(year);
        bookCard.appendChild(description);
        bookCard.appendChild(bookImage);
        bookCard.appendChild(reserveButton);

        bookList.appendChild(bookCard);
    });
}


function toggleReserveForm() {
    const form = document.getElementById("reservation-block");
    form.classList.toggle("show-form");
} 

// Adds the book to the backend
async function addBookWithImage(event) {
    event.preventDefault(); // Prevent page reload

    const allowedExtensions = ['jpg', 'jpeg', 'png'];
    const fileExtension = (document.getElementById('cover-image').files[0]).name.split('.').pop().toLowerCase();

    if (!allowedExtensions.includes(fileExtension)) {
        document.getElementById('message').textContent = 'Invalid file type. Please upload a .jpg, .jpeg, or .png image.';
        return;
    }

    const formData = new FormData();
    formData.append('title', document.getElementById('title').value);
    formData.append('description', document.getElementById('description').value);
    formData.append('year', document.getElementById('year').value);
    formData.append('coverImage', document.getElementById('cover-image').files[0]);

    try {
        const response = await fetch('http://localhost:5201/api/Library/add-book', {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            throw new Error('Error: ' + response.statusText);
        }

        const result = await response.json();
        console.log('Book added successfully:', result);

        const alertBox = document.getElementById('message');

        alertBox.textContent = 'Book added successfully!';
        alertBox.style.display = 'block';
        alertBox.style.opacity = '1';

        setTimeout(() => {
            alertBox.style.opacity = '0';
            setTimeout(() => {
                alertBox.style.display = 'none';
            }, 500);
        }, 1000);


        // fetch and display all books again to reflect the new addition
        fetchBooks();

    } catch (error) {
        console.error('Failed to add book:', error);
        document.getElementById('message').textContent = 'Failed to add book: ' + error.message;
    }
}

// Reserves the book and shows the price of the book as a pop up
async function fetchBook(bookId) {
    const days = document.getElementById("day-number").value;
    const type = document.getElementById("book-type").value;
    const quick = document.getElementById("pickup-type").checked;

    const url = `http://localhost:5201/api/Library/${bookId}?days=${days}&type=${type}&quick=${quick}`;
    
    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error("Failed to fetch price");
        }
        const data = await response.json();
        //console.log(data);
        const priceDiv = document.getElementById("book-price-div");
        priceDiv.textContent = `Price of book you just ordered: ${data.price}`

        priceDiv.style.display = 'block';
        priceDiv.style.opacity = '1';


        setTimeout(() => {
            priceDiv.style.opacity = '0';
            setTimeout(() => {
                priceDiv.style.display = 'none';
            }, 2500);
        }, 2500)


        //document.getElementById("reservation-price").textContent = `Price: $${data.price}`;

        

    } catch (error) {
        console.error("Error fetching book price:", error);
    }
}



