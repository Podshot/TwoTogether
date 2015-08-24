import os
import zipfile
import time
import glob
import getpass

def zipdir(path, ziph):
    for root, dirs, files in os.walk(path):
        for f in files:
            ziph.write(os.path.join(root, f))

def zipStuff(user):
    for f in glob.glob("C:\\Users\\{}\\Documents\\Github\\podshot.github.io\\TwoTogether\\*.zip".format(user)):
        os.remove(f)
    for f in glob.glob("C:\\Users\\{}\\Documents\\Unity\\Two Together\\Builds\\*.zip".format(user)):
        os.remove(f)
    for f in glob.glob("C:\\Users\\{}\\Dropbox\\TwoTogether\\*.zip".format(user)):
        os.remove(f)

    basedir = os.path.abspath(os.curdir)
    print os.path.abspath(basedir)

    win_64 = zipfile.ZipFile("C:\\Users\\{}\\Dropbox\\TwoTogether\\Windows_64bit.zip".format(user), 'w')
    os.chdir("Windows")
    zipdir("TwoTogether_Windows_64bit_Data", win_64)
    win_64.write("TwoTogether_Windows_64bit.exe")
    win_64.close()

    win_32 = zipfile.ZipFile("C:\\Users\\{}\\Dropbox\\TwoTogether\\Windows_32bit.zip".format(user), 'w')
    zipdir("TwoTogether_Windows_32bit_Data", win_32)
    win_32.write("TwoTogether_Windows_32bit.exe")
    win_32.close()

    os.chdir(basedir)
    '''
    os.chdir("Mac")
    osx = zipfile.ZipFile("C:\\Users\\Ben\\Dropbox\\TwoTogether\\OSX_Universal.zip", 'w')
    zipdir("TwoTogether_OSX_Universal.app", osx)
    osx.close()

    os.chdir(basedir)
    '''

    os.chdir("Linux")
    linux = zipfile.ZipFile("C:\\Users\\{}\\Dropbox\\TwoTogether\\Linux_Universal.zip".format(user), 'w')
    zipdir("TwoTogether_Linux_Universal_Data", linux)
    linux.write("TwoTogether_Linux_Universal.x86")
    linux.write("TwoTogether_Linux_Universal.x86_64")
    linux.close()

    fp_template = open("C:\\Users\\{}\\Documents\\Github\\podshot.github.io\\TwoTogether\\downloads_template.html".format(user), 'r')
    template = fp_template.readlines()
    fp_template.close()

    if os.path.exists("C:\\Users\\{}\\Documents\\Github\\podshot.github.io\\TwoTogether\\downloads.html".format(user)):
        os.remove("C:\\Users\\{}\\Documents\\Github\\podshot.github.io\\TwoTogether\\downloads.html".format(user))

    fp = open("C:\\Users\\{}\\Documents\\Github\\podshot.github.io\\TwoTogether\\downloads.html".format(user), 'wb')
    newlines = []
    str_time = time.ctime()
    print str_time
    for line in template:
        newlines.append(line.replace("<datetime>", str_time))

    fp.writelines(newlines)
    fp.close()

zipStuff(getpass.getuser())
_ = raw_input("Finished packaging releases")
