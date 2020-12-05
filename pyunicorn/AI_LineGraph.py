#!/usr/bin/env python
# coding: utf-8

# In[1]:


from pyunicorn.timeseries.surrogates import Surrogates
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

with open('Humble1121_1815.txt') as file:
    row = []
    for line in file:
        sp = line.strip().split('\t')
        row.append(list(map(float, sp[:3])))

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

#IAAFT
#d = Surrogates(original_data=td, silence_level=2).refined_AAFT_surrogates(td,10000)


rms = d[1]
angle = d[2]

count = len(rms)

tIndex = np.linspace(0, count/10, count)


plt.ylabel("Amplitude")
plt.xlabel("Time [s]")
plt.plot(tIndex, rms, tIndex, angle, 'r-')
plt.show()


# In[ ]:




