from fastapi import FastAPI, File, UploadFile, Header
from fastapi.responses import JSONResponse
from typing import Optional
import pytesseract
from PIL import Image
import spacy
import io
import time

app = FastAPI()

def extract_text_and_entities(img, ocr_language='eng', spacy_model='en_core_web_sm') -> dict:
    # pytesseract.pytesseract.tesseract_cmd = r'C:\Users\neo_z\AppData\Local\Programs\Tesseract-OCR\tesseract.exe'
    # Perform OCR recognition using Tesseract
    start = time.time()
    recognized_text = pytesseract.image_to_string(img, lang=ocr_language)

    # Load the spaCy model
    nlp = spacy.load(spacy_model)

    # Process the recognized text using spaCy
    doc = nlp(recognized_text)
    
    elapsed_time = time.time() - start
    # Group entities by label
    entities = {}
    for ent in doc.ents:
        if ent.label_ not in entities:
            entities[ent.label_] = []
        entities[ent.label_].append(ent.text)

    # Add processing time to the result
    result = {'entities': entities, 'processing_time': elapsed_time}

    return result

@app.post("/process/")
async def process(file: UploadFile = File(...), accept_language: Optional[str] = Header(None)):
    # Convert the uploaded file to a PIL Image object
    image_stream = io.BytesIO(await file.read())
    image_stream.seek(0)
    img = Image.open(image_stream)

    # Determine the OCR and spaCy model based on the Accept-Language header
    if accept_language:
        if 'de' in accept_language:
            ocr_language = 'deu'
            spacy_model = 'de_core_news_sm'
        elif 'zh' in accept_language:
            ocr_language = 'chi_sim'
            spacy_model = 'zh_core_web_sm'
        else:
            ocr_language = 'eng'
            spacy_model = 'en_core_web_sm'
    else:
        # Default language settings
        ocr_language = 'eng'
        spacy_model = 'en_core_web_sm'

    # Call the previously defined function to process the image
    result = extract_text_and_entities(img, ocr_language=ocr_language, spacy_model=spacy_model)

    return JSONResponse(content=result)

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000,reload=True, debug=True)
