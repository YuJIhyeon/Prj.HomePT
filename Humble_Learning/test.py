import numpy as np

x=np.array([10,3,41,12,9,1,3,34])
y=(x-np.mean(x))/np.std(x)

print(np.mean(y))