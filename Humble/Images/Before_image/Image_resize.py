import os
from PIL import Image

def resize_image(image_path, save_path):
    image = Image.open(image_path)
    after_img = image.resize((227,227))
    after_img.save(save_path+'.png')

image_path = './biceps/'
save_path = '../After_image/biceps/'
for top, dir, files in os.walk(image_path):
    for i, file in enumerate(files):
        resize_image(image_path+file, save_path+str(i))

image_path = './triceps/'
save_path = '../After_image/triceps/'
for top, dir, files in os.walk(image_path):
    for i, file in enumerate(files):
        resize_image(image_path+file, save_path+str(i))

image_path = './biceps_midAng/'
save_path = '../After_image/biceps_midAng/'
for top, dir, files in os.walk(image_path):
    for i, file in enumerate(files):
        resize_image(image_path+file, save_path+str(i))

image_path = './triceps_midAng/'
save_path = '../After_image/triceps_midAng/'
for top, dir, files in os.walk(image_path):
    for i, file in enumerate(files):
        resize_image(image_path+file, save_path+str(i))

#resize_image()