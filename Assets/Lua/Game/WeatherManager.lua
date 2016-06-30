module("WeatherManager",package.seeall);

weatherType = {
	Sunny = 1,
	Rainy = 2
}

local weather = nil;

function InitializeWeather()
	local percent = math.Random(0,1);
	print(percent);
	if(percent<0.5)then
		weather = weatherType.Sunny;
	else
		weather = weatherType.Rainy;
	end
end

function GetWeather()
	if(weather==nil)then
		InitializeWeather();
	end
	return weather;
end