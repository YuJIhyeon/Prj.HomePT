import tensorflow as tf
from tensorflow import keras

import numpy as np
import matplotlib.pyplot as plt

from tensorflow.compat.v1 import ConfigProto
from tensorflow.compat.v1 import InteractiveSession

config = ConfigProto()
config.gpu_options.allow_growth = 0.4
session = InteractiveSession(config=config)

# 파일 불러오기
import os, re, glob
import cv2

image_size = 227

groups_folder_path = '../Images/After_AAFT/'

X_s = []

y = []

for label in ['Bi', 'Tri']:

    for feature in ['striping_color']:

        for top, dir, f in os.walk(groups_folder_path + label + "/" + feature + "/"):
            for filename in f:
                img = cv2.imread(groups_folder_path + label + "/" + feature + "/" + filename)
                img = cv2.resize(img, None, fx=image_size / img.shape[0], fy=image_size / img.shape[1])

                #if feature == 'striping_bw':
                X_s.append(img / 256)

                if label == 'Bi':
                    y.append(2)
                else:
                    y.append(3)

print(X_s[0].shape)

'''
X = []

for s in X_s:
    img = []
    for i in range(len(s)):
        #row = np.concatenate((s[i]), axis=1)
        img.append(row)
    X.append(img)
'''
X = np.array(X_s)
y = np.array(y)

# train, test split
from sklearn.model_selection import train_test_split

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42, stratify=y)

# validation split

X_train, X_validation, y_train, y_validation = train_test_split(X_train, y_train, test_size=0.1, random_state=10, stratify=y_train)

AlexNet = keras.models.Sequential([
        keras.layers.Conv2D(256, (6, 6), activation='relu', input_shape=X_train[0].shape),
        keras.layers.BatchNormalization(),
        keras.layers.MaxPooling2D( (2, 2) ),
        keras.layers.Conv2D(384, (3, 3), padding='same', activation='relu'),
        keras.layers.BatchNormalization(),
        keras.layers.Conv2D(384, (3, 3), padding='same', activation='relu'),
        keras.layers.BatchNormalization(),
        keras.layers.Conv2D(256, (3, 3), padding='same', activation='relu'),
        keras.layers.BatchNormalization(),
        keras.layers.MaxPooling2D( (2, 2) ),
        keras.layers.Flatten(),
        keras.layers.Dense(1024, activation='relu'),
        keras.layers.Dense(1024, activation='relu'),
        keras.layers.Dense(len(y_train), activation='softmax')
])

AlexNet.summary()

AlexNet.compile(loss=keras.losses.sparse_categorical_crossentropy,
              optimizer=keras.optimizers.SGD(learning_rate=0.01),
              metrics=[keras.metrics.sparse_categorical_accuracy])

AlexNet_history = AlexNet.fit(X_train, y_train, epochs=100,
                    validation_data=(X_validation, y_validation))

AlexNet_history = AlexNet.fit(X_train, y_train, epochs=10,
                    validation_data=(X_test, y_test))