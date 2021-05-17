clear all;
close all;

% hammer    rvcurl
folder_name = './AAFT(5)_addition/rvcurl/';
list = dir(folder_name);

% 62   122   182    242   302   602   1202
for i = 3:302
   c_name = list(i).name;
   n_name = join(['rvcurl_emg_20ms_trial (', num2str(i-2) ,').txt']);
   movefile([folder_name c_name], [folder_name n_name]);
end
