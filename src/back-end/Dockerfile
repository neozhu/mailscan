# Use Python 3.12 Alpine as the base image
FROM python:3.12-alpine

# Set the working directory in the container
WORKDIR /usr/src/app

# Install build dependencies
# Some Python packages might require a build environment, especially those with C extensions
RUN apk add --no-cache --virtual .build-deps gcc musl-dev

# Install Tesseract OCR and its dependencies
RUN apk add tesseract-ocr

# Install Tesseract OCR language packs for German, English, and Simplified Chinese
RUN apk add tesseract-ocr-deu tesseract-ocr-eng tesseract-ocr-chi-sim

# Download spaCy language models
RUN pip install --no-cache-dir fastapi uvicorn pytesseract Pillow spacy
RUN python -m spacy download en_core_web_sm
RUN python -m spacy download de_core_news_sm
RUN python -m spacy download zh_core_web_sm

# Copy the application code
COPY . .

# Expose the port the app runs on
EXPOSE 8000


# Define the command to run on container start
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]

# Clean up build dependencies
RUN apk del .build-deps
