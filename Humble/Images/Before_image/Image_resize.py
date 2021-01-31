import os
from PIL import Image

def resize_image(image_path, save_path):
    image = Image.open(image_path)
    after_img = image.resize((227,227))
    after_img.save(save_path+'.png')

img_type = ['biceps_bw', 'biceps_color', 'biceps_midAng_bw', 'biceps_midAng_color', 'triceps_bw', 'triceps_color', 'triceps_midAng_bw', 'triceps_midAng_color']
base_path = '../Before_AAFT/'
save_path = '../After_AAFT/'
for folder in img_type:
    for top, dir, files in os.walk(base_path+folder):
        for i, file in enumerate(files):
            resize_image(base_path + folder + '/' + file, save_path + folder + '/' + str(i))

#resize_image()