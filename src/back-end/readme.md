# MailScan Backend service api

This project is a FastAPI application that provides an API for Optical Character Recognition (OCR) and language processing. It uses `pytesseract` for OCR and `spaCy` for natural language processing to extract entities from images. The application supports multiple languages including English, German, and Simplified Chinese.

## Features

- OCR on images with support for multiple languages.
- Text extraction and entity recognition using `spaCy`.
- FastAPI for an easy-to-use API interface.

## Getting Started

### Prerequisites

- Docker

### Building the Docker Container

1. Clone the repository to your local machine.
2. Navigate to the directory containing the Dockerfile.
3. Build the Docker image:

   ```bash
   docker build -t mailscanbackend:latest.
   ```

### Running the Docker Container

Run the Docker container using:

```bash
docker run -d --name mailscanbackend -p 8000:8000 mailscanbackend
```

This command will start the container in detached mode, mapping port 8000 of the container to port 8000 of the host machine.

### Interacting with the API

Once the container is running, the API can be accessed at `http://localhost:8000/docs`.

#### Endpoints

- **POST /process/**: Process an image for OCR and language processing.

  - **Parameters**:
    - `file`: The image file to be processed.
    - `accept_language` (optional, in header): Preferred language for OCR (`de` for German, `zh` for Simplified Chinese, defaults to English).

  - **Example**:

    ```bash
    curl -X 'POST' \
      'http://localhost:8000/process/' \
      -H 'accept: application/json' \
      -H 'Content-Type: multipart/form-data' \
      -H 'accept_language: de' \
      -F 'file=@path_to_image.jpg'
    ```

### Local Development (Optional)

For local development without Docker, ensure Python 3.12 and the required packages are installed:

```bash
pip install -r requirements.txt
```

Then, run the application using:

```bash
uvicorn main:app --reload
```

## License

This project is licensed under the [MIT License](LICENSE).

