#!/usr/bin/env python
# coding: utf-8

# In[1]:


from pyunicorn.timeseries.surrogates import Surrogates
import numpy as np
import os
import pandas as pd
import matplotlib.pyplot as plt

# Biceps original data
def original_data_input(file_list, data_type, output_folder):
    cnt = 0
    for file_name in file_list:
        sp = list()
        with open('./' + data_type + '/' + file_name) as file:
            row = []
            for line in file:
                if line == '\n':
                    break
                sp = line.strip().split('\t')
                row.append(list(map(float, sp[:3])))

        sp.clear()

        t = []
        rms = []
        angle = []
        for r in np.array(row):
            t.append(r[0])
            rms.append(r[1])
            angle.append(r[2])

        count = len(rms)

        tIndex = np.linspace(0, count / 10, count)

        plt.ylabel("Amplitude")
        plt.xlabel("Time [s]")
        plt.ylim(0, 180)
        plt.plot(tIndex, rms, tIndex, angle, 'r-')
        plt.savefig('./' + output_folder + '/' + str(cnt) + '.png')
        cnt += 1

        plt.cla()
        row.clear()


# Biceps augmentation
def augmentation_data(file_list, data_type, output_folder):
    cnt = 0
    for file_name in file_list:
        sp = list()
        with open('./'+data_type+'/' + file_name) as file:
            row = []
            for line in file:
                if line == '\n':
                    break
                sp = line.strip().split('\t')
                row.append(list(map(float, sp[:3])))

        sp.clear()

        t = []
        data1 = []
        data2 = []
        for r in np.array(row):
            t.append(r[0])
            data1.append(r[1])
            data2.append(r[2])

        td = np.array([])
        td = np.append(td, t, axis=0)
        td = np.vstack([td, data1])
        td = np.vstack([td, data2])

        # AAFT
        d = Surrogates(original_data=td, silence_level=2).AAFT_surrogates(td)

        # IAAFT
        # d = Surrogates(original_data=td, silence_level=2).refined_AAFT_surrogates(td,10000)

        rms = d[1]
        angle = d[2]

        count = len(rms)

        tIndex = np.linspace(0, count / 10, count)

        plt.ylabel("Amplitude")
        plt.xlabel("Time [s]")
        plt.ylim(0, 180)
        plt.plot(tIndex, rms, tIndex, angle, 'r-')
        plt.savefig('./'+output_folder+'/' + str(cnt) + '.png')
        cnt += 1

        plt.cla()
        row.clear()

def main():
    # Biceps
    path_dir1 = 'C:/Users/Hojin/Desktop/Prj.HomePT/pyunicorn/data_2do'
    file_list1 = os.listdir(path_dir1)

    original_data_input(file_list1, 'data_2do', 'result_original_2do')
    augmentation_data(file_list1, 'data_2do', 'result_augmentation_2do')

    # Triceps
    path_dir2 = 'C:/Users/Hojin/Desktop/Prj.HomePT/pyunicorn/data_3do'
    file_list2 = os.listdir(path_dir2)

    original_data_input(file_list2, 'data_3do', 'result_original_3do')
    augmentation_data(file_list2, 'data_3do', 'result_augmentation_3do')

    print("Finished")

main()


# In[ ]:




