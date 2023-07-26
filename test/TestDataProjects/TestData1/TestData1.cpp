
extern "C"
{
	int TestCMethod1()
	{
		return 10 + 32;
	}
}

class TestClass1
{
private:
	int m_data;

public:

	TestClass1()
	{
		m_data = 42;
	}

	int AddData(int moreData)
	{
		return m_data + moreData;
	}

};

