all:
	make clean
	mcs -out:my_executable.exe Program.cs
clean:
	rm -f my_executable.exe
