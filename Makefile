
all:
	mcs *.cs */*.cs -d:NET_2_1 -pkg:wcf -d:VOCALOID_SHARP_HACK -t:library -out:Commons.VocaloidApi.dll

clean:
	rm Commons.VocaloidApi.dll
