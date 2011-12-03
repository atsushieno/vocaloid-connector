
-- in seconds
local socket_timeout = 60000

function main (processParam, envParam)
	io.lines ("socket-connector.lua")
	package.path = envParam.scriptDir .. '\lua;' .. package.path
	local socket = require ("socket")
	VSDlgSetDialogTitle (package.path)
	VSDlgDoModal ()

	n = {
		noteNum = 80,
		velocity = 100,
		durTick = 480
	}

	VSInsertNote (n)

	return 0

	--[[
	local socket = require ("socket")
	local server = socket.bind ("*", 2039)
	print ("server bound")
	
	while true do
		local client = server.accept ()
		client.settimeout (socket_timeout)
		local line, err = client.receive ()
		if err then
			VSMessageBox (err, MB_OK)
		elseif line == "DONE" then
			break;
		else
			VSMessageBox (line, MB_OK)
		end
	end

	server.close ()

	return 0
	--]]
end

function manifest()

	mf = {
		name = "vocaloid-connector",
		comment = "Accept programmable changes via .NET custom code",
		author = "atsushieno",
		pluginID = "{a1e9fdd6-9061-4179-9256-4411edccc110}",
		pluginVersion = "0.1.0.0",
		apiVersion = "3.0.0.1"
	}
	return mf
end
