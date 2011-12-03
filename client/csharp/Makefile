
all:
	mcs *.cs System.*/*.cs -d:NET_2_1 -pkg:wcf -d:VOCALOID_SHARP_HACK -t:library -out:Commons.VocaloidApi.dll

test:
	mcs samples/*.cs -r:Commons.VocaloidApi.dll -out:test-socket-client.exe

clean:
	rm Commons.VocaloidApi.dll
