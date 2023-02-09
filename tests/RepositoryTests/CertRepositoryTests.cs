﻿namespace RepositoryTests
{
	public class CertRepositoryTests : IClassFixture<MongoDbRandomFixture>
	{
        private ICertRepository _certRepository;
        private MongoDbRandomFixture _dbFixture;

        public CertRepositoryTests(MongoDbRandomFixture dbFixture)
		{
            _dbFixture = dbFixture;
            _certRepository = new MongoDbCert(_dbFixture.Db);
        }

        [Fact]
        public async void AddandGetCertsTest()
        {
            var certs = new CertModel();
            certs.PrivateKey = "-----BEGIN RSA PRIVATE KEY-----\nMIIEowIBAAKCAQEA0N1QyeaDjFHJMaI8Fn54TrWXmWgmswZjytKNw0r1uRFXNY8aBh4PIP9mXc/h\nMN43q7HM3yXpb6Dhi/6ZxB4Qnc4IOwQ/52LD5XiWrSsViuadlZnEcoaejhxLs2ITmtE2kNF43KsW\nAN+4YfNY3o/rUshF0zfDDc87YGEta7BNJUC4VOJiSFhX00b6T7CchxuFMD7iKhhHfEqiaRyVtCVB\nYRYvwJpaSXAPaw4uMIsJRS5DcNWAnX2tfAK3bry6qYQcmoH37M8ZDg6Wq9Rz3xyoe4yiSkFL4na9\nxmGSOAXeJOgdlZAOZt0vFyZicz+OkDTNQwIPpeBD6wAWCIbsFajW5QIDAQABAoIBABXdyF/4Lfcq\nTCXR8M2AEb4CcpZ2b3AYWqk4xZZXtNsFDubGDmOWkQno2IGOH8Al0O6SOjsgdSHL1jZ5rK/PX/j4\nFtIq6I/OaL46PjgZ8HLuyuUHHf3JdzRfOlOo5cQ0ZAHhqZrHSedEhMFdyy+Id2AJeeSaNrsC4kSo\nV9Ypindm8HOH7BYIzOWN7fuWoGtPuMBEXemyTiTXw/eJb2/fRm3izO8gk1FLtkeEnRJO/wQG3VJp\nt89i05FNHcKtayzG0LSZDFXzbYpOjiOM72UyRF1aMGHuylSrTmdkCqgmn5bhH0etS55nhsMaf3/A\nkL97Q3RZCQZ4ob2vIua6MxWvTF8CgYEA+lWZC/PHrhO3Xu6kte8QJ0KBCwW/LlBZYYrt9zDYiySB\nLYQLL1dczY7aAyLDNxIBoq/zbR/9mO0U50TsJra7My3s+w7RpPkWyvEPuaBH81l7882aKIg2HQ8A\nioyvzDP0xifIvPtJpqkZYuQSRiM6Gk+t+QoO3TQLNDW6VeBQbqsCgYEA1ZdyflxkbT2i9sdszFwt\n3X81T6Y7w9qASaS55C43JzwLf3gIiA+kuCXC1NLjXQ22PpBzjSPqqxru6vEmkVEdXe6vCJucAKTc\nt0kAzQulXpdIg/9BT8LJq4RQaxpyCEJAIPSU/hcREtArjwmMvoVX3jpFNK5y/Tkz9DRT121HkK8C\ngYAJxVmJ6+P5WW+o1uu65i/dOG1M/tZtelliw0dyhIePNseL/UFqgaBvrYm44Zx5A8zoirGXqs39\nMBPGyxvkjvFTmBFN41AvfkFzfmE8v5LPXsjFxVqlBlwGWlWNEtyxvb1qFpdxOOWxCZyiDYDhF/Lm\noAGMXOYjoCpnyUkSnQTGrQKBgQCSRLRTdFIRvTPsa4VGLZn7JTIM6XnkFa3kwLnMWRnL9IKrODgf\ndRcIRFO4CvNItisnjSSUcxQxOLCEk8Alo7bIrLuQ2X9rsXq0yXmS8Xa94Dv4qMTBKlOQ8Xtg3Sta\nHIRuDRA7MPCwQX6S5adMkWQq+xyJhy2X26SIh40i6eMoYQKBgGXbQh5A1wr6daIkFePd1UEzpSj6\nNItwUGzzmvo/x9YuxL58VvHa0fFt74bh1ja9T5qrXqe8GSRGl2EpANvicGrMlSX/evyB36bPJFzi\nuSVkHioQmULiNtOK8VUOl42HezgYZ6qmtHa2pPbHXkxrqjdxKVyRjtMYXYAm3OL6vpzp\n-----END RSA PRIVATE KEY-----";
            certs.PublicKey = "-----BEGIN RSA PUBLIC KEY-----\nMIIBCgKCAQEA0N1QyeaDjFHJMaI8Fn54TrWXmWgmswZjytKNw0r1uRFXNY8aBh4PIP9mXc/hMN43\nq7HM3yXpb6Dhi/6ZxB4Qnc4IOwQ/52LD5XiWrSsViuadlZnEcoaejhxLs2ITmtE2kNF43KsWAN+4\nYfNY3o/rUshF0zfDDc87YGEta7BNJUC4VOJiSFhX00b6T7CchxuFMD7iKhhHfEqiaRyVtCVBYRYv\nwJpaSXAPaw4uMIsJRS5DcNWAnX2tfAK3bry6qYQcmoH37M8ZDg6Wq9Rz3xyoe4yiSkFL4na9xmGS\nOAXeJOgdlZAOZt0vFyZicz+OkDTNQwIPpeBD6wAWCIbsFajW5QIDAQAB\n-----END RSA PUBLIC KEY-----";
            await _certRepository.Create(certs.PrivateKey,
                certs.PublicKey);
            var cert = await _certRepository.Read(1);
            Assert.Equal(1, cert.Generation);

            await _certRepository.Create(certs.PrivateKey,
                certs.PublicKey);
            var certsList = await _certRepository.ReadAll();
            Assert.Equal(2, certsList.Count);
        }
    }
}

