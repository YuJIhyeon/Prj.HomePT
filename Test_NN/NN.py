import matplotlib.pyplot as plt
import cv2
import os
import numpy as np
import random
import tensorflow as tf
from tensorflow import keras

def sigmoid(x):
    return 1/(1+np.exp(-x))

# color로 읽으려면 cv2.IMREAD_COLOR
# grayscale -> cv2.IMREAD_GRAYSCALE
images = [[], []]
for root, dirs, files in os.walk('../Humble/Images/After_image/Bi/striping_bw/'):
    for file in files:
       img = cv2.imread(os.path.join(root, file), cv2.IMREAD_GRAYSCALE)
       images[0].append(img)

for root, dirs, files in os.walk('../Humble/Images/After_image/Tri/striping_bw/'):
    for file in files:
       img = cv2.imread(os.path.join(root, file), cv2.IMREAD_GRAYSCALE)
       images[1].append(img)

#random.shuffle(images[0])
#random.shuffle(images[1])

train = []
valid = []
test = []

# 전체 개수의 80% => train
for i in range(int(len(images[0]) * 0.8)):
    train.append(images[0].pop())

for i in range(int(len(images[1]) * 0.8)):
    train.append(images[1].pop())

# 남은 개수의 50% => valid
for i in range(int(len(images[0]) * 0.5)):
    valid.append(images[0].pop())

for i in range(int(len(images[1]) * 0.5)):
    valid.append(images[1].pop())

train = np.array(train)
valid = np.array(valid)
test = np.array(test)

# 나머지 => test
test = images[0]
test.extend(images[1])

# (train, valid, test) = (8, 1, 1)
print(len(train), len(valid), len(test))
print(train[0].shape)

# 라벨링
ans_label = ['Biceps', 'Triceps']
train_label = []
valid_label = []
test_label = []

for i in range(48):
    train_label.append(ans_label[0])
for i in range(47):
    train_label.append(ans_label[1])

for i in range(6):
    valid_label.append(ans_label[0])
for i in range(6):
    valid_label.append(ans_label[1])

for i in range(6):
    test_label.append(ans_label[0])
for i in range(6):
    test_label.append(ans_label[1])

train_label = np.array(train_label)

for var in train:
    var = var / 255.0

for var in valid:
    var = var / 255.0

for var in test:
    var = var / 255.0

model = keras.Sequential([
    keras.layers.Flatten(input_shape=(227, 227)),
    keras.layers.Dense(128, activation='relu'),
    keras.layers.Dense(2, activation='softmax')
])

model.compile(optimizer='adam', loss='sparse_categorical_crossentropy', metrics=['accuracy'])
model.fit(train, train_label, epochs=5)