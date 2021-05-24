clear all
clc
close all

filename = 'C:\Program Files\MATLAB\R2013a\bin\UK Sensor 2046 Readings.csv';
Data = csvread(filename);
Original_color = Data(:,2);
tachoCount = Data(:,1);
plot(tachoCount,Original_color,'b')
hold on
%---------------------------------------------------------------------------

window_size=15;
for i=1:length(Original_color)-window_size
    averaged_light(i)=sum(Original_color(i:i+window_size-1))/window_size;
    tachoCount2(i)=tachoCount(i);   
    
end
plot(tachoCount2,averaged_light,'r')

%---------------------------------------------------------------------------
% hold off

for i=1:length(averaged_light)-1
    derivative_light(i)= abs(averaged_light(i)-averaged_light(i+1)); 
    
end
% figure
for i=1:length(derivative_light)
    if derivative_light(i)<1
        derivative_light(i)=0;
    end
end

window_size2=10;
for i=1:length(derivative_light)-window_size2
    averaged_derivative_light(i)=sum(derivative_light(i:i+window_size2-1))/window_size2;
    tachoCount3(i)=tachoCount2(i);     
    
end

plot(tachoCount3,averaged_derivative_light)

%---------------------------------------------------------------------------
[pks,loc]=findpeaks(averaged_derivative_light);

for i=1:length(loc)-1
    region(i)=loc(i+1)-loc(i);
end

%---------------------------------------------------------------------------

for i=1:length(loc)-1    %find an average light value between peaks
    regionColor(i)=(averaged_light(loc(i)+50));
    if regionColor(i)>80
        regionColorName{i}='Yellow';
    elseif regionColor(i)>60
        regionColorName{i}='Red';
    else
        regionColorName{i}='Black';
    end
    
end

for i=1:length(region)
    regionLength(i)=loc(i+1)-loc(i);
     if regionLength(i)>180
        regionLengthType(i)='3';
    elseif regionLength(i)>100
        regionLengthType(i)='2';
    else
        regionLengthType(i)='1';
    end                  
    
end

%---------------------------------------------------------------------------


for i=1:length(region)
% disp('Region:----------------------------------------')

displayString = [' Region ',num2str(i),' is ',regionColorName{i},' of Width ',regionLengthType(i)];
disp(displayString)
end

%---------------------------------------------------------------------------












