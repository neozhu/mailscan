# Use Python 3.12 Ubuntu as the base image
FROM python:3.12-slim

# Set the working directory in the container
WORKDIR /usr/src/app

# Install Tesseract OCR and its dependencies
RUN apt-get update \
    && apt-get install -y tesseract-ocr \
    && apt-get install -y tesseract-ocr-deu tesseract-ocr-eng tesseract-ocr-chi-sim

# Copy the requirements file
COPY requirements.txt ./

# Install Python dependencies
RUN pip install --no-cache-dir -r requirements.txt


# Download spaCy language models
RUN python -m spacy download en_core_web_sm
RUN python -m spacy download de_core_news_sm
RUN python -m spacy download zh_core_web_sm

# Copy the application code

COPY . .

# Expose the port the app runs on
EXPOSE 8000

# Define the command to run on container start
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]
