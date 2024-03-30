all:
	make clean
	gmcs -out:my_executable.exe Program.cs
clean:
	rm -f my_executable.exe
