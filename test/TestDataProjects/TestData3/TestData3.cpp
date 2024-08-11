// TestData3.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "TestData3.h"


// This is an example of an exported variable
TESTDATA3_API int nTestData3=0;

// This is an example of an exported function.
TESTDATA3_API int fnTestData3(void)
{
    return 0;
}

// This is the constructor of a class that has been exported.
CTestData3::CTestData3()
{
    return;
}
