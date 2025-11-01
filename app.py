from flask import Flask, request, jsonify
from tensorflow.keras.models import load_model
import numpy as np
from PIL import Image
import io

# Flask uygulaması
app = Flask(__name__)

# Modeli yükle
model = load_model("alzheimer_model.h5")

# Veri setindeki sınıf isimleri (doğru sırada)
class_names = ["Mild Demented", "Moderate Demented", "Non Demented", "Very Mild Demented"]

# Modelin eğitimde kullandığı görüntü boyutu
IMG_SIZE = 224  # Eğitim sırasında kullanılan IMG_SIZE ile aynı olmalı

def prepare_image(img_bytes):
    """
    Upload edilen resmi modele uygun hale getirir
    """
    img = Image.open(io.BytesIO(img_bytes)).convert('RGB')
    img = img.resize((IMG_SIZE, IMG_SIZE))  # model ile aynı boyut
    img_array = np.array(img) / 255.0  # normalize et
    img_array = np.expand_dims(img_array, axis=0)  # batch boyutu ekle
    return img_array

@app.route("/predict", methods=["POST"])
def predict():
    if 'file' not in request.files:
        return jsonify({"error": "No file provided"}), 400

    file = request.files['file']
    img_array = prepare_image(file.read())

    # Tahmin
    preds = model.predict(img_array)
    class_index = int(np.argmax(preds[0]))
    confidence = float(preds[0][class_index])
    class_name = class_names[class_index]

    return jsonify({
        "class": class_name,
        "confidence": confidence
    })

if __name__ == "__main__":
    app.run(debug=True)
